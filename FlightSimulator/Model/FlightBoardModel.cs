using FlightSimulator.ViewModels;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    public class FlightBoardModel : ModelNotify
    {
        private SettingsView settingsView;
        public volatile Server server;
        public volatile Client client;
        public  FlightBoardModel ()
        {
            settingsView = new SettingsView();
        }
        
        //showing the setting window
        public void ShowSettings()
        {
            settingsView.Close();
            settingsView = new SettingsView();
            settingsView.Show();
        }
        //connect simulator by Server-Client methodology
        public void ConnectSimulator()
        {
            //connect server
            server = Server.Instance;
            Thread openServerThread = new Thread(server.Start);
            openServerThread.Start();

            client = Client.Instance;
            Thread connectClient = new Thread(client.Start);
            //wait server has a connection
            while (!server.HasConnection()) ;

            //connect client
            connectClient.Start();
        }
    }
}
