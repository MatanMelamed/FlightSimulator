using System;
using System.Collections.Generic;

namespace FlightSimulator.ViewModels {
    class ManualControllerViewModel {

        Client m_client;

        public ManualControllerViewModel() {
            m_client = Client.Instance;
        }

        private float throttle;
        public float Throttle {
            get { return (float)(Math.Truncate((double)throttle * 10.0) / 10.0); }
            set {
                throttle = value;
                SendCommandToClient("set /controls/engines/current-engine/throttle ", value);
            }
        }

        private float rudder;
        public float Rudder {
            get { return (float)(Math.Truncate((double)rudder * 10.0) / 10.0); }
            set {
                rudder = value;
                SendCommandToClient("set /controls/flight/rudder ", value);
            }
        }

        private double aileron;
        public double Aileron {
            get { return Math.Truncate(aileron * 10.0) / 10.0; }
            set {
                aileron = value;
                SendCommandToClient("set /controls/flight/aileron ", value);
            }
        }

        private double elevator;
        public double Elevator {
            get { return Math.Truncate(elevator * 10.0) / 10.0; }
            set {
                elevator = value;
                SendCommandToClient("set /controls/flight/elevator ", value);
            }
        }

        void SendCommandToClient(string commandName,double value) {
            string command = commandName + value.ToString();
            List<string> lst = new List<string>();
            lst.Add(command);

            if (m_client.IsConnected) {
                m_client.SendToSimulator(lst);
            }            
        }
    }
}
