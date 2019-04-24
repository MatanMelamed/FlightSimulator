using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading;
using FlightSimulator.ViewModels;

namespace FlightSimulator.Model {
    public class AutoPilotModel : ModelNotify {
        string _command_text;
        volatile Brush _command_background;
        Client _client;
        public AutoPilotModel() {
            _client = Client.Instance;
        }
        //Properties

        //TextBox text property
        public string Text_Changed {
            get {
                return _command_text;
            }
            set {
                _command_text = value;
                //change the backround to a busy color - Pink
                Background_Changed = Brushes.Pink;
                //notify change
                NotifyPropertyChanged("Text_Changed");
            }
        }
        //TextBox background property
        public Brush Background_Changed {
            get {
                return _command_background;
            }
            set {
                _command_background = value;
                //notify change
                NotifyPropertyChanged("Background_Changed");
            }
        }
        //send the commands to the simulator
        public void Send_Commands() {

            
            //split the textbox to a list of commands
            List<string> commands = Text_Changed.Split('\n').ToList<string>();

            ManualResetEvent commandsSent = new ManualResetEvent(false);

            _client.SendToSimulator(commands, true, commandsSent);

            commandsSent.WaitOne();
            //set the commands to the client
            //_client.SetCommands(commands);

            //send commnads to the simulator
            //_client.Send();

            //changing the background back to transparent
            Background_Changed = Brushes.Transparent;

        }
    }
}
