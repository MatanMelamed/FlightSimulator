using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace FlightSimulator {
    public class Client {
    
        #region Client members
        private TcpClient _client;
        private string _ip;
        private int _port;
        #endregion

        private volatile bool _sending_status;

        #region Singletone
        private static Client m_Instance = null;
        public static Client Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = new Client();
                    m_Instance._client = new TcpClient();
                }
                return m_Instance;
            }
        }
        #endregion

        public void UpdateConnectionInfo() {
            _ip = Properties.Settings.Default.FlightServerIP;
            _port = Properties.Settings.Default.FlightInfoPort;
        }

        //Connect to the simulator
        public void Connect() {
            UpdateConnectionInfo();
            _client.Connect(_ip, _port);
            Console.WriteLine("Sending in ip: " + _ip + " on port: " + _port);
        }

        public void Disconnect() {
            _client.Close();
        }

        //sending commands to the simulator
        public void Send(string[] commands) {

            _sending_status = true;

            NetworkStream networkStream = _client.GetStream();
            ASCIIEncoding encoding = new ASCIIEncoding();

            foreach (string command in commands) {
                byte[] buffer = encoding.GetBytes(command + "\r\n");
                networkStream.Write(buffer, 0, buffer.Length);
                networkStream.Flush();
                Thread.Sleep(2000); //wait 2 seconds each command
            }

            _sending_status = false;
        }

        //Send a single Command
        void SendCommand(string command) {

        }

        public bool IsConnected() {
            return _client.Connected;
        }

        //indication to see if we are still sending commands
        public bool Is_Sending() {
            return _sending_status;
        }
    }
}