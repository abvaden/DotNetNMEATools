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

        private Listener _Listener;
        private StringBuilder _PortHistoryASCIIBuilder;
        private int _NumberOfLines;
        private List<int> _CharactersPerLine;
        private int _MaxNumberOfLines;

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
            _NumberOfLines++;
            _CharactersPerLine.Add(sentence.Length);
            _PortHistoryASCIIBuilder.Append(sentence.Replace(_Listener.LineEndChars,""));

            if(_NumberOfLines > _MaxNumberOfLines)
            {
                _NumberOfLines--;
                _PortHistoryASCIIBuilder.Remove(0, _CharactersPerLine[0]);
                _CharactersPerLine.RemoveAt(0);
            }

            RaisePropertyChanged("PortHistoryASCII");
        }
    }
}
