using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Decoder.Words;

namespace NMEA_Tools.GPS_DataLogger
{
    public abstract class DataSource : IObservable<DataSource>
    {
        #region Public Properties
        public bool IsPushBased
        {
            get
            {
                return _IsPushBased;
            }
            set
            {
                _IsPushBased = value;
            }
        }
        #endregion

        public Dictionary<string, Word> CurrentWords;

        #region Private Fields
        private bool _IsPushBased;
        private ICollection<IObserver<DataSource>> _Subscribers;
        #endregion

        public DataSource(bool isPushBased)
        {
            _IsPushBased = isPushBased;
        }

        #region Public Methods
        public IDisposable Subscribe(IObserver<DataSource> observer)
        {
            if(!_Subscribers.Contains(observer))
            {
                _Subscribers.Add(observer);
            }

            ///TODO: Develop an unsubscriber for data sources
            return null;
        }

        public void NotifySubscribers()
        {
            foreach(IObserver<DataSource> subscriber in _Subscribers)
            {
                subscriber.OnNext(this);
            }
        }
        #endregion
    }
}
