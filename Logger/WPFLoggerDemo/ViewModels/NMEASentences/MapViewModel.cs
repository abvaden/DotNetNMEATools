using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Serial;
using NMEA_Tools.Decoder;
using System.IO;

namespace WPFLoggerDemo.ViewModels.NMEASentences
{
    public class MapViewModel : ViewModelBase, IListenerSubscriber, IDisposable
    {
        public string BrowserAddress
        {
            get
            {
                /*
                string key = "AIzaSyC3xLwB_Z7chKbaSvWzqZxljXGa-TGo8IM";
                string page = string.Format("<iframe src=\"https://www.google.com/maps/embed/v1/view?center={0},{1}&key={2} allowfullscreen />",
                              Longitude,
                              Latitude,
                              key);
                return page;
                */
                string value = "file://" + _LocalPage;
                return value.Replace("\\","/");
            }
            set
            {
                _BrowserAddress = value;
            }
        }
        public string Longitude { get; private set; }
        public string Latitude { get; private set; }
        public string BrowserHeight { get; set; }
        public string BrowserWidth { get; set; }

        public CefSharp.Wpf.ChromiumWebBrowser Browser { get; set; }

        #region Private Fields
        private Listener _Listener;

        // example google maps link https://www.google.com/maps/place/@4916.45,N,12311.12,W
        private string _GoolgeMaps = @"https://www.google.com/maps/place/@";
        private string _GoogleMapsLocation;
        private string _BrowserAddress;

        private DateTime _LastUpdate;
        private TimeSpan _MinUpdateRate;

        private string _LocalPage;
        #endregion

        public MapViewModel(Listener listener)
        {
            _MinUpdateRate = new TimeSpan(0, 0, 3);
            _LastUpdate = DateTime.Now;
            _Listener = listener;
            _Listener.Subscribe(this);
            _LocalPage = Path.GetTempPath() + "GPSMapPage.html";
        }

        public void SentenceReceived(string sentence)
        {
            
            if (sentence.Contains("GPGLL"))
            {
                try
                {
                    NMEA_Tools.Decoder.Sentences.GPGLL gpgll = new NMEA_Tools.Decoder.Sentences.GPGLL(sentence);

                    _GoogleMapsLocation = String.Format("{0},{1}",
                    gpgll.Longitude.Value, gpgll.Latitude.Value);

                    Longitude = gpgll.Longitude.Value;
                    Latitude = gpgll.Latitude.Value;

                    if ((DateTime.Now - _LastUpdate) > _MinUpdateRate)
                    {
                        _UpdateLocalPage();
                        _LastUpdate = DateTime.Now;
                        RaisePropertyChanged("BrowserAddress");
                        RaisePropertyChanged("Longitude");
                        RaisePropertyChanged("Latitude");
                    }
                }
                catch
                {
                    Longitude = "No lock yet";
                    Latitude = "No lock yet";
                }
            }
        }

        private void _UpdateLocalPage()
        {
            string html = String.Format(
                    @"<DOCTYPE html>
<html>
<head>
	<title>Test Page</title>
</head>
<body>
	<iframe src=""https://www.google.com/maps/embed/v1/place?q={0},{1}&key=AIzaSyC3xLwB_Z7chKbaSvWzqZxljXGa-TGo8IM"" />
</body>
</html> ",
                    Latitude.Replace(",", ""),
                    Longitude.Replace(",", ""));

            File.Delete(_LocalPage);
            using (FileStream fileStream = System.IO.File.Create(_LocalPage))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(html);
                }
            }

        }

        public void Dispose()
        {
            System.IO.File.Delete(_LocalPage);
        }
    }
}
