using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    public abstract class Word
    {
		public string Name { get{ return _Name; } }
		private string _Name;

        internal Word(string name)
        {
            _Name = name;
        }
    }
}
