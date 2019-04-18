using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading;
using FlightSimulator.ViewModels;

namespace FlightSimulator.Model
{
    public class AutoPilotModel : ModelNotify
    {
        string _command_text;
        volatile Brush _command_background;
        volatile Client _client;
        public AutoPilotModel()
        {
            _client = new Client();
        }
        //Properties

        //TextBox text property
        public string Text_Changed
        {
            get
            {
                return _command_text;
            }
            set
            {
                _command_text =value;
                //notify change
                NotifyPropertyChanged("Text_Changed");
            }
        }
        //TextBox background property
        public Brush Background_Changed
        {
            get
            {
                return _command_background;
            }
            set
            {
                _command_background = value;
                //notify change
                NotifyPropertyChanged("Background_Changed");
            }
        }

        //send the commands to the simulator
        public void Send_Commands()
        {
            Background_Changed = Brushes.Pink;
            //split the textbox to a list of commands
            string[] commands = Text_Changed.Split('\n');

            //set the commands to the client
            _client.SetCommands(commands);
            //Thread sendCommandThread = new Thread(_client.Send);
            //sending the commands on a differrent thread
            //sendCommandThread.Start();
            _client.Send();

            //changing the background of textbox by another thread

            Background_Changed = Brushes.Transparent;

        }
    }
}
