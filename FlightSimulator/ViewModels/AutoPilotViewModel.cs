using FlightSimulator.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Utils;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : INotifyPropertyChanged
    {
        private AutoPilotModel _apm;
        public AutoPilotViewModel(AutoPilotModel apm)
        {
            _apm = apm;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
