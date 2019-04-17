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
        private SettingsView settingsView;
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
        public CommandHandler OpenSettingsCommand
        {
            get
            {
                return _openSettings ?? (_openSettings = new CommandHandler(() => ShowSettings()));
            }
        }
        public void ShowSettings()
        {
            settingsView.Show();
        }
    }
}
