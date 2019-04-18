using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        private ApplicationSettingsModel _asm;
        public CommandHandler _saveSettings;
        public CommandHandler _reloadSettings;
        public Client client;
        public SettingsViewModel()
        {
            _asm = new ApplicationSettingsModel();
        }
        //Properties

        //The IP of the server
        public string FlightServerIP
        {
            get
            {
                return _asm.FlightServerIP;
            }
            set
            {
                _asm.FlightServerIP = value;
                //notify change
                OnPropertyChanged("ServerIP");
            }
        }
        //The port of the server
        public int FlightInfoPort
        {
            get
            {
                return _asm.FlightInfoPort;
            }
            set
            {
                _asm.FlightInfoPort = value;
                //notify change
                OnPropertyChanged("ServerPort");
            }
        }
        //the port of the client
        public int FlightCommandPort
        {
            get
            {
                return _asm.FlightCommandPort;
            }
            set
            {
                _asm.FlightCommandPort = value;
                //notify change
                OnPropertyChanged("ClientPort");
            }
        }
        //command to save the settings
        public CommandHandler Save_Settings
        {
            get
            {
                return _saveSettings?? (_saveSettings=new CommandHandler(()=>SaveSettings()));
            }
        }
        //command not to save the settings
        public CommandHandler Reload_Settings
        {
            get
            {
                return _reloadSettings ?? (_reloadSettings = new CommandHandler(() => ReloadSettings()));
            }
        }
        //save the settings
        public void SaveSettings()
        {
            _asm.SaveSettings();
            
        }
        //not saving the settings
        public void ReloadSettings()
        {
            _asm.ReloadSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
