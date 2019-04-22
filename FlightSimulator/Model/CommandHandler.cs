using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FlightSimulator.Model {
    using System.Windows;
    using System.Windows.Forms;

    public class CommandHandler : ICommand {

        Timer _timer;
        System.Windows.Controls.Button _button;
        private Action _action;

        public CommandHandler(Action action) {
            _timer = new System.Windows.Forms.Timer();
            _button = null;
            _action = action;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void ButtonTimerEnded(object sender, System.EventArgs args) {
            _button.IsEnabled = true;
            _timer.Stop();
        }

        public void Execute(object parameter) {

            _action();

            if (parameter is Window) {
                Window window = parameter as Window;
                window.Hide();
            }
            else if (parameter is System.Windows.Controls.Button) {
                _button = _button ?? parameter as System.Windows.Controls.Button;
                _button.IsEnabled = false;

                _timer.Interval = 1000;
                _timer.Tick += ButtonTimerEnded;
                _timer.Start();
            }
        }
    }
}
