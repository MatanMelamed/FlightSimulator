using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading;

namespace FlightSimulator.Model
{
    public class AutoPilotModel : INotifyPropertyChanged
    {
        string _command_text;
        Brush _command_background;
        public AutoPilotModel()
        {
            _command_text = "";
        }
        public event PropertyChangedEventHandler PropertyChanged;

       

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Command_Text
        {
            get
            {
                return _command_text;
            }
            set
            {
                _command_text =value;
                OnPropertyChanged("Text Changed");
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
        public void ClearCommands()
        {
            Command_Text = "";
            OnPropertyChanged("Text Changed");
        }

    }
}
