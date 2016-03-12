using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEADecoder.Words
{
    public class Latitude : Word
    {
		public string Value { get{ return _Value; } }
		private string _Value;

        public double Minutes
        {
            get
            {
                return 0;
            }
        }
        public double Seconds
        {
            get
            {
                return 0;
            }
        }
        public double Degrees
        {
            get
            {
                return 0;
            }
        }

        public Latitude(string value) : base("Latitude")
        {
            _Value = value;
        }
    }
}
