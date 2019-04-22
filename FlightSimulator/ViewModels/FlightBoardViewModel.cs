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

namespace FlightSimulator.ViewModels {
    public class FlightBoardViewModel : BaseNotify {

        private CommandHandler _openSettings;
        private CommandHandler _connectSimulator;
        public FlightBoardModel _flightBoardModel;

        public FlightBoardViewModel() {
            _flightBoardModel = new FlightBoardModel();

            //Tie and chain the Notify of the viewModel with the model Notify - when the model notify, the viewModel notify As well.
            _flightBoardModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

        //Longtitude and Latitude values are drawn and set from the model
        public double Lon {
            get { return _flightBoardModel.Lon; }
            set { _flightBoardModel.Lon = value; }
        }

        public double Lat {
            get { return _flightBoardModel.Lat; }
            set { _flightBoardModel.Lat = value; }
        }

        public CommandHandler OpenSettingsCommand {
            get {
                return _openSettings ?? (_openSettings = new CommandHandler(() => ShowSettings()));
            }
        }

        public void ShowSettings() {
            _flightBoardModel.ShowSettings();
        }

        public CommandHandler ConnectCommnad {
            get {
                return _connectSimulator ?? (_connectSimulator = new CommandHandler(() => ConnectSimulator()));
            }
        }

        public void ConnectSimulator() {
            _flightBoardModel.ConnectSimulator();
        }
    }
}
    
/*
set controls/flight/rudder -1
set controls/flight/rudder 1
set controls/flight/rudder -1
set controls/flight/rudder 1
*/
