using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLoggerDemo.ViewModels.NMEASentences
{
    public class DataViewModelBase : ViewModels.ViewModelBase, NMEA_Tools.Serial.IListenerSubscriber, IDisposable
    {
        public string Name { get; set; }

        internal NMEA_Tools.Serial.Listener _Listener;

        public DataViewModelBase(NMEA_Tools.Serial.Listener listener, string name)
        {
            _Listener = listener;
            Name = name;
        }

        public void Enable()
        {
            _Listener.Subscribe(this);

        }

        public void Close()
        {
            Dispose();
        }

        public virtual void SentenceReceived(string sentence)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _Listener.Unsubscribe(this);
        }

        public new string ToString()
        {
            return Name;
        }
    }
}
