using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    public class FixTime : Word
    {
		public string Value { get{ return _Value; } }
		public DateTime TimeValue { get{ return _TimeValue; } }
		private string _Value;
		private DateTime _TimeValue;

        public byte Hours { get { return _Hours; } }
        public byte Minutes { get { return _Minutes; } }
        public float Seconds { get { return _Seconds; } }
        private byte _Hours;
        private byte _Minutes;
		private float _Seconds;

        public FixTime(string value) : base("Fix Time")
        {
            _Value = value;

			double testVar;
			if( !double.TryParse(value,out testVar))
            {
                throw new Exceptions.WordFormatException("The length of the Fix Time is not valid, only values are accepted, " + value + " was submitted", null);
            }


            if( !Byte.TryParse(value.Substring(0, 2), out _Hours) && (_Hours <= 24))
            {
                throw new Exceptions.WordFormatException("The format for the Hours value is invalid value must be two digits and <= 24 " + value.Substring(0, 2) + " provided", null);
            }
            if (!Byte.TryParse(value.Substring(2, 2), out _Minutes) && (_Minutes <= 60))
            {
                throw new Exceptions.WordFormatException("The format for the Minutes value is invalid value must be two digits and <= 60 " + value.Substring(2, 2) + " provided", null);
            }
            if (!float.TryParse(value.Substring(4, 2), out _Seconds) && (_Seconds <=  60))
            {
                throw new Exceptions.WordFormatException("The format for the Seconds value is invalid value must be two digits and <= 60 " + value.Substring(4, 2) + " provided", null);
            }
			_TimeValue = new DateTime(1, 1, 1, _Hours, _Minutes, (int)_Seconds);
        }
    }
}
