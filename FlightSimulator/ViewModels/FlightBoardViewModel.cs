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
        public FlightBoardViewModel ()
        {
            _fbm = FlightBoardModel.Instance;
            //Notify the fit property in the view model
            _fbm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }
        #endregion
        public double Lon
        {
            get
            {
                return _fbm.Lon ;
            }
            set
            {
                _fbm.Lon = value;

            }
        }

        public double Lat
        {
            get
            {
                return _fbm.Lat;
            }
            set
            {
                _fbm.Lat = value;
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
