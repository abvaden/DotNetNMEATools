using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Serial;
using NMEA_Tools.Decoder;

namespace WPFLoggerDemo.ViewModels.NMEASentences
{
    public class MapViewModel : ViewModelBase, IListenerSubscriber
    {
        public string BrowserPage
        {
            get
            {
                return _GoolgeMaps + _GoogleMapsLocation;
            }
        }
        public string Longitude { get; private set; }
        public string Latitude { get; private set; }

        private Listener _Listener;

        // example google maps link https://www.google.com/maps/place/4916.45,N,12311.12,W
        private string _GoolgeMaps = @"https://www.google.com/maps/place/";
        private string _GoogleMapsLocation;

        private DateTime _LastUpdate;
        private TimeSpan _MinUpdateRate;

        public MapViewModel(Listener listener)
        {
            _MinUpdateRate = new TimeSpan(0, 0, 3);
            _LastUpdate = DateTime.Now;
            _Listener = listener;
            _Listener.Subscribe(this);
        }

        public void SentenceReceived(string sentence)
        {
            
            if (sentence.Contains("GPGLL"))
            {
                NMEA_Tools.Decoder.Sentences.GPGLL gpgll = new NMEA_Tools.Decoder.Sentences.GPGLL(sentence);

                _GoogleMapsLocation = String.Format("{0},{1}",
                    gpgll.Longitude.Value, gpgll.Latitude.Value);

                Longitude = gpgll.Longitude.Value;
                Latitude = gpgll.Latitude.Value;

                if(gpgll.ActiveState.CurrentState == NMEA_Tools.Decoder.Words.ActiveState.ActiveStateEnum.DataActive)
                {
                    if((DateTime.Now - _LastUpdate) > _MinUpdateRate)
                    {
                        _LastUpdate = DateTime.Now;
                        RaisePropertyChanged("BrowserPage");
                        RaisePropertyChanged("Longitude");
                        RaisePropertyChanged("Latitude");
                    }
                }
            }

        }
    }
}
