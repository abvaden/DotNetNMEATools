using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    public class GroundSpeed : Word
    {
        public double Speed
        {
            get
            {
                return 0;
            }
            set
            {
                _Speed = value;
            }
        }

        private double _Speed;

        public GroundSpeed(string vlaue) : base("Gorund Speed")
        {

        }
    }
}
