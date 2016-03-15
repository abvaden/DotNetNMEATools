using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace WPFLoggerDemo.ViewModels
{
    public class SerialPortListenerSetupViewModel : INotifyPropertyChanged
    {
        public double StopBits { get; set; }
        public double DataBits { get; set; }

        public Array Paritys { get; private set; }
        public double[] AvaialbeBaudRates { get; private set; }
        public string[] AvailablePorts
        {
            get
            {
                return _Listener.GetAvailablePorts();
            }
        }

        public string SelectedPort { get; set; }
        public double SelectedBaudRate { get; set; }
        public Parity SelectedParity { get; set; }

        private NMEA_Tools.Serial.Listener _Listener;

        public SerialPortListenerSetupViewModel()
        {
            #region Populate the data for the combo boxes
            AvaialbeBaudRates = new double[]
            {
                110, 150, 300,
                1200, 2400, 4800,
                9600, 19200, 38400,
                57600, 115200, 230400,
                460800, 921600
            };
            Paritys = Enum.GetValues(typeof(System.IO.Ports.Parity));
            #endregion

            // Set the defaults
            DataBits = 8;
            StopBits = 1;
            SelectedBaudRate = AvaialbeBaudRates[5];
            SelectedParity = Parity.None;
            _Listener = new NMEA_Tools.Serial.Listener();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
        #endregion INotifyPropertyChanged
    }
}
