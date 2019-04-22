using FlightSimulator.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator {
    class Server {

        #region Server members
        private TcpListener _listener;
        private IPAddress _ip;
        private int _port;
        private Task _clientHandlerTask;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _taskToken;
        public bool HasConnection { get; private set; }
        #endregion

        FlightBoardModel flightBoardModel;

        private static Server m_Instance = null;
        public static Server Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new Server();
                    m_Instance.HasConnection = false;
                }
                return m_Instance;
            }
        }

        public void UpdateConnectionInfo() {
            _ip = IPAddress.Parse(Properties.Settings.Default.FlightServerIP);
            _port = Properties.Settings.Default.FlightInfoPort;
        }

        public void SetFlightBoardModel(FlightBoardModel newModel) {
            flightBoardModel = newModel;
        }

        public TcpClient ListenForAClient() {
            _listener = new TcpListener(_ip, _port);
            Console.WriteLine("Listening with ip: " + _ip.ToString() + " on port: " + _port);
            _listener.Start();

            //wait till we have a connection
            TcpClient tcpClient = _listener.AcceptTcpClient();
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
                Console.WriteLine("Received : " + dataReceived);

                //getting the lon and the lat
                string[] values = dataReceived.Split(',');
                flightBoardModel.Lon = Convert.ToDouble(values[0]);
                flightBoardModel.Lat = Convert.ToDouble(values[1]);
            }
            client.Close();
            _listener.Stop();
            HasConnection = false;
        }

        public void Start() {
            UpdateConnectionInfo();
            TcpClient client = ListenForAClient();
            _tokenSource = new CancellationTokenSource();
            _taskToken = _tokenSource.Token;
            _clientHandlerTask = Task.Run(() => HandleClient(client), _taskToken);
        }

        public void Stop() {
            _tokenSource.Cancel();
        }


        /***
 * 
 * Task oldClientHandlerTask = _clientHandlerTask;

            CancellationTokenSource newSource = new CancellationTokenSource();
            CancellationToken newToken = newSource.Token;

            _clientHandlerTask = Task.Run(() => {
                oldClientHandlerTask.Wait();
                _tokenSource.Dispose();
                _tokenSource = newSource;
                _taskToken = newToken;
                UpdateConnectionInfo();
                TcpClient client = ListenForAClient();
                HandleClient(client);
            }, newToken);
 ***/

        public void Reset() {
            Stop();
            _clientHandlerTask.Wait();
            _tokenSource.Dispose();
            Start();
        }
    }
}

