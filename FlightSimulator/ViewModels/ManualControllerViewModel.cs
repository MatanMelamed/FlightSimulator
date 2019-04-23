using System;

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
                SendCommandToClient("throttle ",value);
            }
        }

        private float rudder;
        public float Rudder {
            get { return (float)(Math.Truncate((double)rudder * 10.0) / 10.0); }
            set {
                rudder = value;
                SendCommandToClient("rudder ",value);
            }
        }

        private double aileron;
        public double Aileron {
            get { return Math.Truncate(aileron * 10.0) / 10.0; }
            set {
                aileron = value;
                SendCommandToClient("aileron ",value);
            }
        }

        private double elevator;
        public double Elevator {
            get { return Math.Truncate(elevator * 10.0) / 10.0; }
            set {
                elevator = value;
                SendCommandToClient("elevator ",value);
            }
        }

        void SendCommandToClient(string commandName,double value) {
            string command = commandName + value.ToString();

            if (m_client.IsConnected) {
                m_client.SendToSimulator(null, command);
            }            
        }
    }
}
