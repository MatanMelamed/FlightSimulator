using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator {

    public class CommandPackage {
        public List<string> commands { get; set; }
        public ManualResetEvent finishedEvent { get; set; }

        public CommandPackage(List<string> newCommands, ManualResetEvent newFinishedEvent = null) {
            this.commands = newCommands;
            if (newFinishedEvent == null) {
                this.finishedEvent = new ManualResetEvent(false);
            }
            else {
                this.finishedEvent = newFinishedEvent;
            }
        }
    }

    public class Client {

        #region Network members
        TcpClient _client;
        string _ip;
        int _port;
        public bool IsConnected { get; private set; }
        #endregion

        #region Tasks management members
        CancellationTokenSource _tokenSource;
        CancellationToken _taskToken;
        Task _startClientTask = null;
        Task _stopClientTask = null;
        #endregion

        #region Client job members
        ConcurrentQueue<CommandPackage> commands;
        public ManualResetEvent GotCommands;
        #endregion

        #region Singletone
        private static Client m_Instance = null;
        public static Client Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new Client();
                    m_Instance._client = new TcpClient();
                    m_Instance.commands = new ConcurrentQueue<CommandPackage>();
                    m_Instance.GotCommands = new ManualResetEvent(false);
                }
                return m_Instance;
            }
        }
        #endregion

        void UpdateConnectionInfo() {
            _ip = Properties.Settings.Default.FlightServerIP;
            _port = Properties.Settings.Default.FlightCommandPort;
        }

        void ConnectToTarget() {
            UpdateConnectionInfo();
            _client.Connect(_ip, _port);
            Console.WriteLine("Sending in ip: " + _ip + " on port: " + _port);
            IsConnected = true;
        }

        /***
         * Add command or commands to send to the simulator
         * In case of both valid input, insert list first and then single command.
         * */
        //public void SendToSimulator(List<string> newCommands, string newCommand = null) {

        //    if (newCommands != null) {
        //        foreach (string command in newCommands) {
        //            commands.Enqueue(newCommand);
        //        }
        //    }

        //    if(newCommand != null) {
        //        commands.Enqueue(newCommand);
        //    }

        //    // checks if the event is ready to be fired
        //    // event can only get ready by main loop when main loop is sleeping
        //    if (!GotCommands.WaitOne(0)) {
        //        GotCommands.Set();
        //    }
        //}

        public void SendToSimulator(List<string> newCommands, ManualResetEvent newFinishedEvent = null) {

            CommandPackage newPackage = new CommandPackage(newCommands); ;

            if (newFinishedEvent != null) {
                newPackage.finishedEvent = newFinishedEvent;
            }

            commands.Enqueue(newPackage);

            // checks if the event is ready to be fired
            // event can only get ready by main loop when main loop is sleeping
            if (!GotCommands.WaitOne(0)) {
                GotCommands.Set();
            }
        }

        /***
         * Main client loop - wake up when getting a new mission.
         * for each existing mission, dequeue it and send it to the network stream.
         * should make sure the function is not sleeping in order to cancel it.
         ***/
        void RunMainLoop() {
            CommandPackage package;
            NetworkStream networkStream = _client.GetStream();
            ASCIIEncoding encoding = new ASCIIEncoding();

            while (!_taskToken.IsCancellationRequested) {
                while (!commands.IsEmpty && !_taskToken.IsCancellationRequested) {
                    commands.TryDequeue(out package);
                    foreach (string commmand in package.commands) {
                        byte[] buffer = encoding.GetBytes(commmand + "\r\n");
                        networkStream.Write(buffer, 0, buffer.Length);
                        networkStream.Flush();
                        Thread.Sleep(2000); //wait 2 seconds each command
                    }

                    if(package.finishedEvent != null) {
                        package.finishedEvent.Set();
                    }
                }
                GotCommands.Reset();
                GotCommands.WaitOne();
            }
        }

        //Connect to the simulator
        public void Connect() {
            if (_startClientTask != null && _startClientTask.Status == TaskStatus.Running) {
                return;
            }

            _startClientTask = Task.Run(() => {
                // Check if the server has already a connection, else sleep until event happen
                //if (!Server.Instance.HasConnection) {
                //    Server.Instance.GotConnected.WaitOne();
                //}

                // check if disconnect thread is running, and wait for it to finish before continuing
                if (_stopClientTask != null) {
                    _stopClientTask.Wait();
                }

                ConnectToTarget();
                RunMainLoop();
            }, _taskToken);
        }

        public void Disconnect() {
            if (_startClientTask == null || _startClientTask.Status != TaskStatus.Running) {
                return;
            }

            _stopClientTask = Task.Run(() => {
                _tokenSource.Cancel();
                // if main loop is waiting for event, set it on to wake it up
                if (GotCommands.WaitOne(0)) {
                    GotCommands.Set();
                }
                _startClientTask.Wait();
                _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource();
                _taskToken = _tokenSource.Token;
                IsConnected = false;
            });
        }
    }
}