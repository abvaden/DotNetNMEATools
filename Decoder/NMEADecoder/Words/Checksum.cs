using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    public class Checksum : Word
    {
		public string Value { get{ return _Value; } }
		public byte ConvertedValue { get{ return _ConvertedValue; } }
		private string _Value;
		private byte _ConvertedValue;

        public Checksum(string value) : base("CheckSum")
        {
            byte byteValue;
            if(Byte.TryParse(value,System.Globalization.NumberStyles.HexNumber,null, out byteValue))
            {
                _Value = value;
                _ConvertedValue = byteValue;
            }
            
        }
    }
}
