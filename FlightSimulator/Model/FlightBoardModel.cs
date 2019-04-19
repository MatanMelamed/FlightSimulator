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
        public static FlightBoardModel m_Instance = null;
        private double _lon = 0;
        private double _lat = 0;
        public  static FlightBoardModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightBoardModel();
                    m_Instance.settingsView = new SettingsView();
                }
                return m_Instance;
            }
        }

        //Lon property
        public double Lon
        {
            get
            {
                return _lon;
            }
            set
            {
                _lon = value;
            }
        }

        //Lat property
        public double Lat
        {
            get
            {
                return _lat;
            }
            set
            {
                _lat = value;
                //Notify change to the viewmodel
                NotifyPropertyChanged("Lat");
            }
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
            

            //connect client
            connectClient.Start();
        }
    }
}
