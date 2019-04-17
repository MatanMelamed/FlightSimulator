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
    class Server
    {
        private TcpListener server;
        public Server()
        {
            IPAddress IP = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(IP, 5400);
            Console.WriteLine("Listening");
            /*Thread tcpListenerThread = new Thread(Start);
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();*/
        }
        public void Start()
        {
            server.Start();
            TcpClient client = server.AcceptTcpClient();
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
                //TimeSpan interval = new TimeSpan(0, 0,10);
                //Thread.Sleep(interval);
            }
        }
    }
}   