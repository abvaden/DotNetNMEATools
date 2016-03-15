using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using NMEA_Tools.Serial;
using System.Windows.Input;

namespace WPFLoggerDemo.ViewModels
{
    public class SerialPortListenerSetupViewModel : INotifyPropertyChanged
    {
        #region Public Properties
        public double StopBits { get; set; }
        public double DataBits { get; set; }
        public string LineEndingCharacters { get; set; }
        public string SelectedPort { get; set; }
        public double SelectedBaudRate { get; set; }
        public Parity SelectedParity { get; set; }

        #region LineEnding Booleans
        public bool? LineEnding_r1Checked
        {
            get
            {
                return _LineEnding_r1Checked;
            }
            set
            {
                _LineEnding_r1Checked = value;
                _LineEnding_n1Checked = !value;
            }
        }
        public bool? LineEnding_n1Checked
        {
            get
            {
                return _LineEnding_n1Checked;
            }
            set
            {
                _LineEnding_n1Checked = value;
                _LineEnding_r1Checked = !value;
            }
        }
        public bool? LineEnding_r2Checked
        {
            get
            {
                return _LineEnding_r2Checked;
            }
            set
            {
                _LineEnding_r2Checked = value;
                _LineEnding_n2Checked = !value;
            }
        }
        public bool? LineEnding_n2Checked
        {
            get
            {
                return _LineEnding_n2Checked;
            }
            set
            {
                _LineEnding_n2Checked = value;
                _LineEnding_r2Checked = !value;
            }
        }
        public bool? LineEnding_r3Checked
        {
            get
            {
                return _LineEnding_r3Checked;
            }
            set
            {
                _LineEnding_r3Checked = value;
                _LineEnding_n3Checked = !value;
            }
        }
        public bool? LineEnding_n3Checked
        {
            get
            {
                return _LineEnding_n3Checked;
            }
            set
            {
                _LineEnding_n3Checked = value;
                _LineEnding_r3Checked = !value;
            }
        }
        #endregion

        public Array Paritys { get; private set; }
        public double[] AvaialbeBaudRates { get; private set; }
        public string[] AvailablePorts
        {
            get
            {
                return _Listener.GetAvailablePorts();
            }
        }
        public ICommand OpenCommand { get; }
        #endregion

        private Listener _Listener;
        private bool? _LineEnding_r1Checked;
        private bool? _LineEnding_n1Checked;
        private bool? _LineEnding_r2Checked;
        private bool? _LineEnding_n2Checked;
        private bool? _LineEnding_r3Checked;
        private bool? _LineEnding_n3Checked;


        public SerialPortListenerSetupViewModel(Listener listener)
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
            LineEndingCharacters = "\r\n";
            DataBits = 8;
            StopBits = 1;
            SelectedBaudRate = AvaialbeBaudRates[5];
            SelectedParity = Parity.None;
            _Listener = listener;

            OpenCommand = new Command
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
        #endregion INotifyPropertyChanged

        private bool OpenCommand_CanExecute()
        {
            return false;
        }

        private void OpenCommand_Execute();
    }
}
