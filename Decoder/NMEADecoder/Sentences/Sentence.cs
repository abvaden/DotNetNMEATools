using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace NMEA_Tools.Decoder.Sentences
{
    public abstract class Sentence : INotifyDataErrorInfo
    {
        #region Public Properties
        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            private set
            {
                _Prefix = value;
            }
        }
        public bool Locked { get; internal set; }
        public Words.Checksum Checksum
        {
            get
            {
                return _Checksum;
            }
            set
            {
                _Checksum = value;
            }
        }
        #endregion

        #region Internal Properties
        internal string _Prefix;
        internal string _SentenceValue;
        internal static char[] _SentenceSplitChars = new char[] { ',','*' };
        private Dictionary<string, Words.Word> _sentenceWords;
        #endregion

        #region Private Fields
        private bool _HasErrors;
        private Words.Checksum _Checksum;
        #endregion

        internal Sentence(string value)
        {
            _SentenceValue = value;

            if (!CheckCheckSum())
            {
                _HasErrors = true;
            }
            else
            {
                _HasErrors = false;
            }

            Locked = true;
        }
        internal Sentence()
        {
            this.Locked = false;
            this._HasErrors = false;
        }


        internal bool CheckCheckSum()
        {
            int indexOfCheckSumSpace = _SentenceValue.LastIndexOf('*');
            int checksum = 0;
            for (int i = 1; i < indexOfCheckSumSpace; i++)
            {
                checksum ^= Convert.ToByte(_SentenceValue[i]);
            }

            if(checksum.ToString("X") == _SentenceValue.Substring(indexOfCheckSumSpace + 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        abstract internal bool SentenceHasErrors();

        public void Unlock()
        {
            Locked = false;
        }

        #region INotidyDataErrorInfo
        public bool HasErrors
        {
            get
            {
                if(Locked)
                {
                    return _HasErrors;
                }
                else
                {
                    // The sentence is unlocked so we must delegate the call to the client 
                    
                    return SentenceHasErrors();
                }
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        #endregion
    }
}
