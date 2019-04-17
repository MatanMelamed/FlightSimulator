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
    class AutoPilotModel : INotifyPropertyChanged
    {
        public AutoPilotModel()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;

       

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
