using FlightSimulator.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Threading;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : INotifyPropertyChanged
    {
        private AutoPilotModel _apm;
        private string _command_text;
        private Brush _command_background;
        public CommandHandler _clearCommand;
        public CommandHandler _sendCommands;
        private Client _client;
        public AutoPilotViewModel()
        { 
            _command_text = "";
            _apm = new AutoPilotModel();
            _client = new Client();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public CommandHandler ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new CommandHandler(() =>
                Clear_Commands()));
            }
        }
        public CommandHandler SendCommands
        {
            get
            {
                return _sendCommands ?? (_sendCommands = new CommandHandler(() =>
                Send_Commands()));
            }
        }

        //Properties
        public string Command_Text
        {
            get
            {
                return _command_text;
            }
            set
            {
                _command_text = value; OnPropertyChanged(_command_text);
            }
        }

        public Brush Commands_Background
        {
            get
            {
                return _command_background;
            }
            set
            {
                _command_background = value;
                OnPropertyChanged("");
            }
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Clear_Commands()
        {
            Command_Text = "";
        }

        public void Send_Commands()
        {
            string[] commands = _command_text.Split('\n');
            _client.SetCommands(commands);
            Thread sendCommandThread = new Thread(_client.Start);
            sendCommandThread.Start();
            Thread ChangeBackgrounThread = new Thread(ChangeBackground);
            ChangeBackgrounThread.Start();

        }
        public void ChangeBackground()
        {
            Commands_Background = Brushes.Pink;
            while (_client.Is_Sending()) { }
            Commands_Background = Brushes.Transparent;
        }

    }
}


/*
set controls/flight/rudder -1
set controls/flight/rudder 1
set controls/flight/rudder -1
set controls/flight/rudder 1
*/