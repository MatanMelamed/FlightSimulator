using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using FlightSimulator.Views;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {
        private CommandHandler _openSettings;
        private CommandHandler _connectSimulator;
        private SettingsView settingsView;
        public volatile Server server;
        public volatile Client client;
        public FlightBoardViewModel()
        {
            settingsView = new SettingsView();
        }

        public double Lon
        {
            get;
        }

        public double Lat
        {
            get;
        }

        //open setting command
        public CommandHandler OpenSettingsCommand
        {
            get
            {
                return _openSettings ?? (_openSettings = new CommandHandler(() => ShowSettings()));
            }
        }

        //showing the setting window
        public void ShowSettings()
        {
            settingsView.Close();
            settingsView = new SettingsView();
            settingsView.Show();
        }

        //command to connect the program to the simulator
        public CommandHandler ConnectCommnad
        {
            get
            {
                return _connectSimulator ?? (_connectSimulator = new CommandHandler(() => ConnectSimulator()));
            }
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
            while (!server.HasConnection());

            //connect client
            connectClient.Start();
        }
    }
}
