using FlightSimulator.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Threading;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : BaseNotify
    {
        private AutoPilotModel autoPilotModel;
        public CommandHandler _clearCommand;
        public CommandHandler _sendCommands;
        public AutoPilotViewModel()
        { 
            autoPilotModel = new AutoPilotModel();
            //Notify the fit property in the view model
            autoPilotModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        //Command to clear textbox
        public CommandHandler ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new CommandHandler(() =>
                Clear_Commands()));
            }
        }

        //Command to send commands to the simulator
        public CommandHandler SendCommands
        {
            get
            {
                return _sendCommands ?? (_sendCommands = new CommandHandler(() =>
                Send_Commands()));
            }
        }

        //Properties

        //TextBox text property
        public string VM_Text_Changed
        {
            get
            {
                return autoPilotModel.Text_Changed;
            }
            set
            {
                autoPilotModel.Text_Changed = value;
                //notify change
                NotifyPropertyChanged("VM_Text_Changed");
            }
        }
        //TextBox background property
        public Brush VM_Background_Changed
        {
            get
            {
                return autoPilotModel.Background_Changed;
            }
            set
            {
                autoPilotModel.Background_Changed = value;
                //notify change
                NotifyPropertyChanged("VM_Background_Changed");
            }
        }
        //clear textbox
        public void Clear_Commands()
        {
            autoPilotModel.ClearCommands();
        }

        //send the commands to the simulator in other thread
        public void Send_Commands()
        {
            Thread sendCommnads= new Thread(autoPilotModel.Send_Commands);
            sendCommnads.Start();
        }
    }
}
