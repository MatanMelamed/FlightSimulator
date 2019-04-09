using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulator.UserControls
{
        /// <summary>
        /// Interaction logic for AutoPilot.xaml
        /// </summary>
    public partial class AutoPilot : UserControl
    {
        AutoPilotViewModel _apvm;
        public AutoPilot()
        {
            InitializeComponent();
            _apvm = new AutoPilotViewModel(new AutoPilotModel());
            DataContext = _apvm;
        }
    }
}
