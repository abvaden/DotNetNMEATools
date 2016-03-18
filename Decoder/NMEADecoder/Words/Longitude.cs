using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    public class Longitude : Word
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
        public enum Heimsphere { North, South };

        private Heimsphere _Hemisphere;
        private double _Minutes;
        private double _Seconds;
        private double _Degrees;

        public Longitude(string value) : base("Longitude")
        {
            _Value = value;
            string[] spiltValue = _Value.Split(new char[] { ',' });

            if (spiltValue.Length != 2)
            {
                return;
            }

            #region Hemisphere
            if (spiltValue[1] == "N")
            {
                _Hemisphere = Heimsphere.North;
            }
            else if (spiltValue[1] == "S")
            {
                _Hemisphere = Heimsphere.South;
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
                if (_Hemisphere == Heimsphere.South)
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
