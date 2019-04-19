using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace FlightSimulator
{
    public class Client
    {
        
        #region Singletone
        private static Client m_Instance = null;
        private string _ip;
        private int _port;
        private TcpClient _client;
        private string[] _commands;
        private volatile bool _sending_status;
        private Server server;

        //constructor
        public static Client Instance 
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new Client();
                    m_Instance._ip = Properties.Settings.Default.FlightServerIP;
                    m_Instance._port = Properties.Settings.Default.FlightCommandPort;
                    m_Instance.server = Server.Instance;
                    //TimeSpan interval = new TimeSpan(0, 0, 60);
                    //Thread.Sleep(interval);
                    //_client.Connect("127.0.0.1", 5402);
                }
                return m_Instance;
            }
            
        }
        #endregion
        //connect client
        public void Start()
        {
            //wait server has a connection
            while (!server.HasConnection()) ;
            //connect to the simulator
            _client = new TcpClient();
            _client.Connect(_ip,_port);
            Console.WriteLine("Sending in ip: " + _ip + " on port: " + _port);
        }
        //sending commands to the simulator
        public void Send()
        {
            
            _sending_status = true;
            NetworkStream networkStream = _client.GetStream();
            ASCIIEncoding encoding = new ASCIIEncoding();
            
            foreach (string command in _commands)
            {
                byte[] buffer = encoding.GetBytes(command+"\r\n");
                networkStream.Write(buffer, 0, buffer.Length);
                networkStream.Flush();
                //wait 2 seconds each command
                TimeSpan interval = new TimeSpan(0, 0, 2);
                Thread.Sleep(interval);
            }
            _sending_status = false;
            
            
            /*
            int x = 1;
            while (true)
            {
                if (x == 1)
                {
                    byte[] buffer = encoding.GetBytes("set controls/flight/rudder 1\r\n");
                    networkStream.Write(buffer, 0, buffer.Length);
                    networkStream.Flush();
                    x = 0;
                } else
                {
                    byte[] buffer = encoding.GetBytes("set controls/flight/rudder -1\r\n");
                    networkStream.Write(buffer, 0, buffer.Length);
                    networkStream.Flush();
                    x = 1;
                }
                TimeSpan interval = new TimeSpan(0, 0, 2);
                Thread.Sleep(interval);
            }
            */
            
        }
        //set the commands to send the simulator
        public void SetCommands(string [] commands)
        {
            _commands = commands;
        }
        //indication to see if we are still sending commands
        public bool Is_Sending()
        {
            return _sending_status;
        }
        public void Set_IP_Port(string ip,int port)
        {
            _ip = ip;
            _port = port;
        }
    }
}
