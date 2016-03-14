using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Serial;

namespace NMEA_Tools.GPS_DataLogger.LoggableSerialListener
{
    public class LoggableSerialListener : DataSource, IListenerSubscriber
    {
        #region Private Fields
        private Listener _listener;
        private IDisposable _ListenerUnsubscriber;
        #endregion


        public LoggableSerialListener(Listener listener) : base(true)
        {
            _listener = listener;
            _ListenerUnsubscriber = _listener.Subscribe(this);

            this.CurrentWords = new Dictionary<string, Decoder.Words.Word>();
        }

        public void SentenceReceived(string sentence)
        {
            try
            {
                Decoder.Sentences.GPGLL gpgll = new Decoder.Sentences.GPGLL(sentence);
                this.CurrentWords[gpgll.Latitude.Name] = gpgll.Latitude;
                this.CurrentWords[gpgll.Longitude.Name] = gpgll.Longitude;
                this.CurrentWords[gpgll.FixTime.Name] = gpgll.FixTime;
                this.CurrentWords[gpgll.ActiveState.Name] = gpgll.ActiveState;
            }
            catch
            {

            }
        }
    }
}
