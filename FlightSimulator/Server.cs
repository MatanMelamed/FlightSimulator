using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator
{
    public class Server
    {
        #region Singletone
        private static Server m_Instance = null;
        private TcpListener server;
        private bool hasConnected;
        private IPAddress _ip;
        private int _port;
        private FlightBoardModel _fbm;
        public static Server Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Server();
                    m_Instance._ip = IPAddress.Parse(Properties.Settings.Default.FlightServerIP);
                    m_Instance._port = Properties.Settings.Default.FlightInfoPort;
                    m_Instance._fbm = FlightBoardModel.Instance;
                }
                return m_Instance;
            }
        }
        #endregion
        //start recieving data
        public void Start()
        {
            //connect to the simulator
            server = new TcpListener(_ip, _port);
            Console.WriteLine("Listening with ip: " + _ip.ToString() + " on port: " + _port);
            server.Start();

            //wait till we have a connection
            TcpClient client = server.AcceptTcpClient();

            //we connected to the simulator
            hasConnected = true;

            while (true)
            {


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
                _fbm.Lon = Convert.ToDouble(values[0]);
                _fbm.Lat = Convert.ToDouble(values[1]);
            }
        }
        //indication to see if we are connected to the simulator
        public bool HasConnection()
        {
            return hasConnected;
        }

        //set the ip and port manually
        public void Set_IP_Port(string ip, int port)
        {
            _ip = IPAddress.Parse(ip);
            _port = port;
        }

    }
}