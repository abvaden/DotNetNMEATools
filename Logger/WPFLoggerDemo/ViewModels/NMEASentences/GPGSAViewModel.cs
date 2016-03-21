using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Serial;
using NMEA_Tools.Decoder.Sentences;
using NMEA_Tools.Decoder.Words;

namespace WPFLoggerDemo.ViewModels.NMEASentences
{
    public class GPGSAViewModel : DataViewModelBase
    {
        public string HDOP { get; private set; }
        public string PDOP { get; private set; }
        public string VDOP { get; private set; }
        public string NumberSats { get; private set; }
        public string Mode { get; private set; }

        private GPGSA _GPGSA;
        public GPGSAViewModel(Listener listener) : base(listener, "GSA Words")
        {

        }

        public override void SentenceReceived(string sentence)
        {
            if(sentence.Contains("$GPGSA"))
            {
                try
                {
                    _GPGSA = new GPGSA(sentence);

                    if(_GPGSA.Dimension == GSADimension.Three)
                    {
                        Mode = _GPGSA.Mode.ToString() + " / 3D"; 
                    }
                    else if(_GPGSA.Dimension == GSADimension.Two)
                    {
                        Mode = _GPGSA.Mode.ToString() + " / 2D";
                    }
                    else
                    {
                        Mode = _GPGSA.Mode.ToString() + " / " + _GPGSA.Dimension.ToString();
                    }
                    
                    HDOP = _GPGSA.HDOP.ToString();
                    VDOP = _GPGSA.VDOP.ToString();
                    PDOP = _GPGSA.PDOP.ToString();
                    NumberSats = _GPGSA.SatIDs.Length.ToString();
                }
                catch
                {
                    HDOP = "N/A";
                    VDOP = "N/A";
                    PDOP = "N/A";
                    NumberSats = "N/A";
                    Mode = "N/A";
                }

                RaisePropertyChanged("HDOP");
                RaisePropertyChanged("PDOP");
                RaisePropertyChanged("VDOP");
                RaisePropertyChanged("NumberSats");
                RaisePropertyChanged("Mode");

            }
        }
    }
}
