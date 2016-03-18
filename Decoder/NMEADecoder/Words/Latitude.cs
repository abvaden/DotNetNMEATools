using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    public class Latitude : Word
    {
        public string Value { get { return _Value; } }
        private string _Value;

        public double Minutes
        {
            get
            {
                return _Minutes;
            }
        }
        public double Seconds
        {
            get
            {
                return _Seconds;
            }
        }
        public double Degrees
        {
            get
            {
                return _Degrees;
            }
        }
        public Heimsphere Hemisphere
        {
            get
            {
                return _Hemisphere;
            }
        }
        public enum Heimsphere { East, West };

        private Heimsphere _Hemisphere;
        private double _Minutes;
        private double _Seconds;
        private double _Degrees;

        public Latitude(string value) : base("Latitude")
        {
            _Value = value;
            string[] spiltValue = _Value.Split(new char[] { ',' });

            if (spiltValue.Length != 2)
            {
                return;
            }

            #region Hemisphere
            if (spiltValue[1] == "E")
            {
                _Hemisphere = Heimsphere.East;
            }
            else if (spiltValue[1] == "W")
            {
                _Hemisphere = Heimsphere.West;
            }
            else
            {
                return;
            }
            #endregion

            #region Minutes / Seconds
            int decimalIndex = spiltValue[0].IndexOf(".");
            if (!Double.TryParse(spiltValue[0].Substring(decimalIndex - 2), out _Minutes))
            {
                // Error parsing value
                return;
            }
            _Seconds = (_Minutes - Math.Floor(_Minutes)) * 60;
            #endregion

            #region Degrees
            if (Double.TryParse(spiltValue[0].Substring(0, decimalIndex - 2), out _Degrees))
            {
                _Degrees += _Minutes / 60;
                if (_Hemisphere == Heimsphere.West)
                {
                    _Degrees = _Degrees * -1;
                }
            }
            else
            {
                // There was an error
            }
            #endregion
        }
    }
}
