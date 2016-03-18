using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Serial;

namespace WPFLoggerDemo.ViewModels
{
    public class RawPortDataViewModel : ViewModelBase, IListenerSubscriber
    {
        public string PortHistoryASCII
        {
            get
            {
                return _PortHistoryASCIIBuilder.ToString();
            }
        }
        public int NumberOfLines
        {
            get
            {
                return _MaxNumberOfLines;
            }
            set
            {
                _MaxNumberOfLines = value;
                lock(_DataLockObject)
                {
                    _PortHistoryASCIIBuilder.Clear();
                    _NumberOfLines = 0;
                }
            }
        }

        private Listener _Listener;
        private StringBuilder _PortHistoryASCIIBuilder;
        private int _NumberOfLines;
        private List<int> _CharactersPerLine;
        private int _MaxNumberOfLines;
        private object _DataLockObject = new object();

        public RawPortDataViewModel(Listener listener)
        {
            _Listener = listener;
            _Listener.Subscribe(this);
            _NumberOfLines = 0;
            _CharactersPerLine = new List<int>();
            _PortHistoryASCIIBuilder = new StringBuilder();
            _MaxNumberOfLines = 20;

        }

        public void SentenceReceived(string sentence)
        {
            lock (_DataLockObject)
            {
                _NumberOfLines++;
                _CharactersPerLine.Add(sentence.Length);
                _PortHistoryASCIIBuilder.Append(sentence.Replace(_Listener.LineEndChars, ""));

                if (_NumberOfLines > _MaxNumberOfLines)
                {
                    _NumberOfLines--;
                    _PortHistoryASCIIBuilder.Remove(0, _CharactersPerLine[0]);
                    _CharactersPerLine.RemoveAt(0);
                }
            }
            RaisePropertyChanged("PortHistoryASCII");
        }
    }
}
