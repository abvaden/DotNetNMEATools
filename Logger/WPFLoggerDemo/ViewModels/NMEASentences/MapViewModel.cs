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
    public class MapViewModel : DataViewModelBase, IListenerSubscriber, IDisposable
    {
        public string BrowserAddress
        {
            get
            {
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
        public string FixTime { get; private set;}

        public CefSharp.Wpf.ChromiumWebBrowser Browser { get; set; }

        #region Private Fields
        private string _BrowserAddress;

        private DateTime _LastUpdate;
        private TimeSpan _MinUpdateRate;

        private string _LocalPage;

        private NMEA_Tools.Decoder.Sentences.GPGLL _GPGLL;
        #endregion

        public MapViewModel(Listener listener) : base(listener, "Map View")
        {
            _MinUpdateRate = new TimeSpan(0, 0, 3);
            _LastUpdate = DateTime.Now;
            _LocalPage = Path.GetTempFileName().Replace(".tmp", ".html");
        }

        public new void SentenceReceived(string sentence)
        {
            
            if (sentence.Contains("GPGLL"))
            {
                try
                {
                    _GPGLL = new NMEA_Tools.Decoder.Sentences.GPGLL(sentence);

                    Longitude = _GPGLL.Longitude.Value;
                    Latitude = _GPGLL.Latitude.Value;
                    FixTime = _GPGLL.FixTime.TimeValue.ToLongTimeString();

                    if ((DateTime.Now - _LastUpdate) > _MinUpdateRate)
                    {
                        _UpdateLocalPage();
                        _LastUpdate = DateTime.Now;
                        RaisePropertyChanged("BrowserAddress");
                        RaisePropertyChanged("Longitude");
                        RaisePropertyChanged("Latitude");
                        RaisePropertyChanged("FixTime");
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
	<iframe width=""{0}"" height=""{1}"" frameborder=""0"" style=""border: 0""
src = ""https://www.google.com/maps/embed/v1/view?zoom={4}&center={2},{3}&key=AIzaSyC3xLwB_Z7chKbaSvWzqZxljXGa-TGo8IM"" allowfullscreen ></iframe>
</body>
</html> ",
                    Convert.ToDouble(BrowserWidth) - 25,
                    Convert.ToDouble(BrowserHeight) - 25,
                    _GPGLL.Longitude.Degrees,
                    _GPGLL.Latitude.Degrees,
                    16);

            File.Delete(_LocalPage);
            _LocalPage = Path.GetTempFileName().Replace(".tmp", ".html");
            using (FileStream fileStream = File.Create(_LocalPage))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(html);
                }
            }

        }

        public new void Dispose()
        {
            File.Delete(_LocalPage);
            base.Dispose();
        }
    }
}
