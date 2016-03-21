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
    public class GPVTGViewModel : DataViewModelBase
    {
        
        public string MagneticHeading { get; private set; }
        public string TrueHeading { get; private set; }
        public string GroundSpeedKnots { get; private set; }
        public string GroundSpeedKph { get; private set; }

        private GPVTG _GPVTG;

        public GPVTGViewModel(Listener listener) : base(listener, "GPVTG Words")
        {
        }

        public override void SentenceReceived(string sentence)
        {
            if (sentence.Contains("$GPVTG"))
            {
                try
                {
                    _GPVTG = new GPVTG(sentence);

                    MagneticHeading = _GPVTG.MagneticTrack.Heading.ToString("F3");
                    TrueHeading = _GPVTG.TrueTrack.Heading.ToString("F3");

                    GroundSpeedKnots = _GPVTG.GroundSpeed.Speed.ToString("F3");
                    GroundSpeedKph = _GPVTG.GroundSpeed.Speed.ToString("F3");
                }
                catch (Exception excpt)
                {
                    string value = "Error with received string";
                    MagneticHeading = value;
                    TrueHeading = value;

                    GroundSpeedKnots = value;
                    GroundSpeedKph = value;
                }

                RaisePropertyChanged("GroundSpeedKnots");
                RaisePropertyChanged("GroundSpeedKph");
                RaisePropertyChanged("TrueHeading");
                RaisePropertyChanged("MagneticHeading");            }
        }
    }
}
