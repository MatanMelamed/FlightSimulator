using FlightSimulator.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlightSimulator.ViewModels;
using System.Windows;

namespace FlightSimulator {
    class Server {

        #region Network members
        private TcpListener _listener;
        private IPAddress _ip;
        private int _port;
        #endregion

        #region Garbage Values 
        const int NUMBER_GARBAGE_VAL = 2;
        private bool garbage_values;
        private int counter_gVal;
        #endregion

        #region Tasks management members
        CancellationTokenSource _tokenSource;
        CancellationToken _taskToken;
        Task _startServerTask = null;
        Task _stopServerTask = null;
        #endregion

        #region Connection events cross threads members
        public bool HasConnection { get; private set; }
        public AutoResetEvent GotConnected;
        #endregion

        FlightBoardModel flightBoardModel;

        #region Singleton
        private static Server m_Instance = null;
        public static Server Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new Server();
                    m_Instance._tokenSource = new CancellationTokenSource();
                    m_Instance._taskToken = m_Instance._tokenSource.Token;
                    m_Instance.GotConnected = new AutoResetEvent(false);
                    m_Instance.HasConnection = false;
                    m_Instance.garbage_values = false;
                    m_Instance.counter_gVal = 0;
                }
                return m_Instance;
            }
        }
        #endregion

        public void UpdateConnectionInfo() {
            _ip = IPAddress.Parse(Properties.Settings.Default.FlightServerIP);
            _port = Properties.Settings.Default.FlightInfoPort;
        }

        public void SetFlightBoardModel(FlightBoardModel newModel) {
            flightBoardModel = newModel;
        }

        public TcpClient ListenForAClient() {
            _listener = new TcpListener(_ip, _port);
            //Console.WriteLine("Listening with ip: " + _ip.ToString() + " on port: " + _port);
            _listener.Start();

            //wait till we have a connection
            //conslog("waiting for clients");
            while (!_listener.Pending()) {
                if (_taskToken.IsCancellationRequested) {
                    //conslog("start task got cancel while waiting for clients");
                    return null;
                }
                Thread.Sleep(500); // choose a number (in milliseconds) that makes sense
            }
            TcpClient tcpClient = _listener.AcceptTcpClient();
            //conslog("start task got client");
            GotConnected.Set();
            HasConnection = true;
            return tcpClient;
        }

        public void HandleClient(TcpClient client) {
            while (!_taskToken.IsCancellationRequested) {

                // Get a stream object for reading 					
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                //Console.WriteLine("Received : " + dataReceived);

                //getting the lon and the lat
                string[] values = dataReceived.Split(',');
                if (garbage_values)
                {
                        flightBoardModel.Lon = Convert.ToDouble(values[0]);
                        flightBoardModel.Lat = Convert.ToDouble(values[1]);
                }
                if (garbage_values == false)
                {
                    counter_gVal++;
                    if (counter_gVal == NUMBER_GARBAGE_VAL)
                    {
                        garbage_values = true;
                    }
                }
            }
            //conslog("handle client finished");
        }

        // Start the server in a new task thread.
        public void Start() {
           // conslog("start task called");
            if (_startServerTask != null && _startServerTask.Status == TaskStatus.Running) {
                return;
            }

            _startServerTask = Task.Run(() => {
                if (_stopServerTask != null) {
                    //conslog("waiting for stop task");
                    _stopServerTask.Wait();
                    //conslog("finished waiting for stop task");
                }

                UpdateConnectionInfo();
                TcpClient client = ListenForAClient();
                if (client != null) {
                    HandleClient(client);
                    client.Close();
                }
            }, _taskToken);
        }

        public void conslog(string message) {
            Console.WriteLine("Server ---- " + message);
        }
        // Stop the server in a new task thread.
        public Task Stop() {
            //conslog("stop called.");
            if (_startServerTask == null || _startServerTask.Status != TaskStatus.Running) {
                return null;
            }

            _stopServerTask = Task.Run(() => {
                //conslog("canceling token");
                _tokenSource.Cancel();
                //conslog("waiting for start task to end");
                _startServerTask.Wait();
                //conslog("finished waiting for start task");
                _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource();
                _taskToken = _tokenSource.Token;
                _listener.Stop();
                HasConnection = false;
                //conslog("finished stop task");
            });
            return _stopServerTask;
        }
    }
}