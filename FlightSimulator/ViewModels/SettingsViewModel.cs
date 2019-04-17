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
        public SettingsViewModel()
        {
            _asm = new ApplicationSettingsModel();
        }

        public string FlightServerIP
        {
            get
            {
                return _asm.FlightServerIP;
            }
            set
            {
                _asm.FlightServerIP = value;
                OnPropertyChanged("ServerIP");
            }
        }
        public int FlightInfoPort
        {
            get
            {
                return _asm.FlightInfoPort;
            }
            set
            {
                _asm.FlightInfoPort = value;
                OnPropertyChanged("ServerPort");
            }
        }
        public int FlightCommandPort
        {
            get
            {
                return _asm.FlightCommandPort;
            }
            set
            {
                _asm.FlightCommandPort = value;
                OnPropertyChanged("ClientPort");
            }
        }
        public CommandHandler Save_Settings
        {
            get
            {
                return _saveSettings?? (_saveSettings=new CommandHandler(()=>SaveSettings()));
            }
        }
        public CommandHandler Reload_Settings
        {
            get
            {
                return _reloadSettings ?? (_reloadSettings = new CommandHandler(() => ReloadSettings()));
            }
        }
        public void SaveSettings()
        {
            _asm.SaveSettings();
            
        }
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
