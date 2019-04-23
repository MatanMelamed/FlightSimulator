using FlightSimulator.ViewModels;
using FlightSimulator.Views;

namespace FlightSimulator.Model {
    public class FlightBoardModel : ModelNotify {

        private SettingsView settingsView = null;

        public FlightBoardModel() {
            settingsView = new SettingsView();
            Server.Instance.SetFlightBoardModel(this);      // set the flight board model at the server.
        }

        private double _lon = 0;
        public double Lon {
            get { return _lon; }
            set {
                _lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        private double _lat = 0;
        public double Lat {
            get { return _lat; }
            set {
                _lat = value;
                NotifyPropertyChanged("Lat");   //Notify change to the viewmodel
            }
        }

        //showing the setting window
        public void ShowSettings() {
            if (settingsView == null || !settingsView.IsLoaded) {
                settingsView = new SettingsView();
            }
            settingsView.Show();
        }

        //connect simulator by Server-Client methodology
        public void ConnectSimulator() {
            
            Server server = Server.Instance;
            if (server.HasConnection) {
                server.Stop();
            }
            server.Start();

            Client client = Client.Instance;
            if (client.IsConnected) {
                client.Disconnect();
            }
            client.Connect();
        }
    }
}
