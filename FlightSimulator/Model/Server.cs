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
        private bool _garbage_lines;
        private int _counter_garbage_lines;
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
                    m_Instance._garbage_lines = true;
                    m_Instance._counter_garbage_lines = 0;
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
            _listener.Start();

            //wait till we have a connection
            while (!_listener.Pending()) {
                if (_taskToken.IsCancellationRequested) {
                    return null;
                }
                Thread.Sleep(500); // choose a number (in milliseconds) that makes sense
            }
            TcpClient tcpClient = _listener.AcceptTcpClient();
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

                //getting the lon and the lat
                string[] values = dataReceived.Split(',');
                if (!_garbage_lines)
                {
                        flightBoardModel.Lon = Convert.ToDouble(values[0]);
                        flightBoardModel.Lat = Convert.ToDouble(values[1]);
                }
                if (_garbage_lines == true)
                {
                    _counter_garbage_lines++;
                    if (_counter_garbage_lines == NUMBER_GARBAGE_VAL)
                    {
                        _garbage_lines = false;
                    }
                }
            }
        }

        // Start the server in a new task thread.
        public void Start() {
            if (_startServerTask != null && _startServerTask.Status == TaskStatus.Running) {
                return;
            }

            _startServerTask = Task.Run(() => {
                if (_stopServerTask != null) {
                    _stopServerTask.Wait();
                }

                UpdateConnectionInfo();
                TcpClient client = ListenForAClient();
                if (client != null) {
                    HandleClient(client);
                    client.Close();
                }
            }, _taskToken);
        }

        // Stop the server in a new task thread.
        public Task Stop() {
            if (_startServerTask == null || _startServerTask.Status != TaskStatus.Running) {
                return null;
            }

            _stopServerTask = Task.Run(() => {
                _tokenSource.Cancel();
                _startServerTask.Wait();
                _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource();
                _taskToken = _tokenSource.Token;
                _listener.Stop();
                HasConnection = false;
            });
            return _stopServerTask;
        }
    }
}