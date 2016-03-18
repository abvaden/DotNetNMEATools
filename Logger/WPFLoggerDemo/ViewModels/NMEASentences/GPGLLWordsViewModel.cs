using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLoggerDemo.ViewModels.NMEASentences
{
    public class GPGLLWordsViewModel : DataViewModelBase
    {
        public string ActiveState { get; private set; }
        public string FixTime { get; private set; }
        public string Longitude { get; private set; }
        public string Latitude { get; private set; }

        private NMEA_Tools.Decoder.Sentences.GPGLL _GPGLL;

        public GPGLLWordsViewModel(NMEA_Tools.Serial.Listener listener) : base(listener, "GPGLL Words")
        {

        }

        public override void SentenceReceived(string sentence)
        {
            if (sentence.Contains("GPGLL"))
            {
                try
                {
                    _GPGLL = new NMEA_Tools.Decoder.Sentences.GPGLL(sentence);
                    ActiveState = _GPGLL.ActiveState.CurrentState.ToString();
                    FixTime = _GPGLL.FixTime.TimeValue.ToLongTimeString();
                    Longitude = _GPGLL.Longitude.Degrees.ToString("F3");
                    Latitude = _GPGLL.Latitude.Degrees.ToString("F3");
                    RaisePropertyChanged("ActiveState");
                    RaisePropertyChanged("FixTime");
                    RaisePropertyChanged("Longitude");
                    RaisePropertyChanged("Latitude");
                }
                catch
                {
                    Longitude = "Not valid sentence";
                    Latitude = "Not valid sentence";
                }
            }
        }
    }
}
