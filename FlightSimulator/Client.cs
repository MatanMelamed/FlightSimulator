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
        private TcpClient _client;
        private string[] _commands;
        private volatile bool _sending_status;
        public Client()
        {
            _client = new TcpClient();
            //TimeSpan interval = new TimeSpan(0, 0, 60);
            //Thread.Sleep(interval);
            //_client.Connect("127.0.0.1", 5402);
            _sending_status = false;
        }
        public void Start()
        {
            _sending_status = true;
            NetworkStream networkStream = _client.GetStream();
            ASCIIEncoding encoding = new ASCIIEncoding();
            foreach (string command in _commands)
            {
                byte[] buffer = encoding.GetBytes(command+"\r\n");
                networkStream.Write(buffer, 0, buffer.Length);
                networkStream.Flush();
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
        public void SetCommands(string [] commands)
        {
            _commands = commands;
        }
        public bool Is_Sending()
        {
            return _sending_status;
        }
    }
}
