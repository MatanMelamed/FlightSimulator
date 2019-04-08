using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Utils
{
    interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
    public delegate void PropertyChangedEventHandler(
        Object sender,
        PropertyChangedEventArgs e);
}
