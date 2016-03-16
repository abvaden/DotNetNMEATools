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
    public class SerialPortListenerSetupViewModel : ViewModelBase
    {
        #region Public Properties
        public StopBits SelectedStopBits { get; set; }
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
                if(value == true)
                {
                    _LineEnding_n1Checked = false;
                }


                RaisePropertyChanged();
                RaisePropertyChanged("LineEnding_n1Checked");
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
                if(value == true)
                {
                    _LineEnding_r1Checked = false;
                }
                

                RaisePropertyChanged();
                RaisePropertyChanged("LineEnding_r1Checked");
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
                if(value == true)
                {
                    _LineEnding_n2Checked = false;
                }
                

                RaisePropertyChanged();
                RaisePropertyChanged("LineEnding_n2Checked");
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
                if(value == true)
                {
                    _LineEnding_r2Checked = false;
                }
                

                RaisePropertyChanged();
                RaisePropertyChanged("LineEnding_r2Checked");
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
                if(value == true)
                {
                    _LineEnding_n3Checked = false;
                }
                

                RaisePropertyChanged();
                RaisePropertyChanged("LineEnding_n3Checked");
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
                if(value == true)
                {
                    _LineEnding_r3Checked = false;
                }
                

                RaisePropertyChanged();
                RaisePropertyChanged("LineEnding_r3Checked");
            }
        }
        #endregion

        public Array Paritys { get; private set; }
        public Array StopBits { get; private set; }
        public double[] AvaialbeBaudRates { get; private set; }
        public string[] AvailablePorts
        {
            get
            {
                return _Listener.GetAvailablePorts();
            }
        }
        public RelayCommand OpenCommand { get; }
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
            Paritys = Enum.GetValues(typeof(Parity));
            StopBits = Enum.GetValues(typeof(StopBits));
            #endregion

            // Set the defaults
            LineEndingCharacters = "\r\n";
            DataBits = 8;
            SelectedBaudRate = AvaialbeBaudRates[5];
            SelectedParity = Parity.None;
            SelectedStopBits = System.IO.Ports.StopBits.One;
            _Listener = listener;

            LineEnding_r1Checked = true;
            LineEnding_n2Checked = true;
            LineEnding_n3Checked = false;
            LineEnding_r3Checked = false;

            OpenCommand = new RelayCommand(OpenCommand_Execute, OpenCommand_CanExecute);
        }

        

        private bool OpenCommand_CanExecute()
        {
            if (this.SelectedPort != null)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private void OpenCommand_Execute()
        {
            _Listener.BaudRate = (int)this.SelectedBaudRate;
            _Listener.DataBits = (int)this.DataBits;
            _Listener.Parity = this.SelectedParity;
            _Listener.PortName = this.SelectedPort;
            _Listener.StopBits = this.SelectedStopBits;
            #region Line Ending
            StringBuilder lineEndingBuilder = new StringBuilder();

            if (LineEnding_n1Checked == true)
            {
                lineEndingBuilder.Append('\n');
            }
            if (LineEnding_r1Checked == true)
            {
                lineEndingBuilder.Append('\r');
            }
            if (LineEnding_n2Checked == true)
            {
                lineEndingBuilder.Append('\n');
            }
            if (LineEnding_r2Checked == true)
            {
                lineEndingBuilder.Append('\r');
            }
            if (LineEnding_n3Checked == true)
            {
                lineEndingBuilder.Append('\n');
            }
            if (LineEnding_r3Checked == true)
            {
                lineEndingBuilder.Append('\r');
            }
            
            _Listener.LineEndChars = lineEndingBuilder.ToString();
            #endregion
            _Listener.SetupPort();
            _Listener.Open();
        }
    }
}
