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
using System.Windows;
using System.ComponentModel;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {
        private CommandHandler _openSettings;
        private CommandHandler _connectSimulator;
        public FlightBoardModel _fbm;
        private double _lon;
        private double _lat;
        #region Singleton
        private static FlightBoardViewModel m_Instance = null;
        public static FlightBoardViewModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightBoardViewModel();
                    /*
                    m_Instance._fbm = new FlightBoardModel();
                    m_Instance._fbm.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
                      {
                          m_Instance._fbm.NotifyPropertyChanged(e.PropertyName);
                      };
                      */
                }
                return m_Instance;
            }
        }
        #endregion
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

        public double Lat
        {
            get
            {
                return _lat;
            }
            set
            {
                _lat = value;
                NotifyPropertyChanged("Lat");
            }
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
            _fbm.ShowSettings();
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
            _fbm.ConnectSimulator();
        }
    }
}
/*
set controls/flight/rudder -1
set controls/flight/rudder 1
set controls/flight/rudder -1
set controls/flight/rudder 1
*/
