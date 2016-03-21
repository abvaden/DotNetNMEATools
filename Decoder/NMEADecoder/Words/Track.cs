using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    /// <summary>
    /// Track 
    /// </summary>
    /// <remarks>
    /// Examples
    /// 054.7,T     True track made good (degrees)
    /// 034.4,M     Magnetic track made good</remarks>
    public class Track : Word
    {
        #region Public Properties
        public TrackType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        public double Heading
        {
            get
            {
                return _Heading;
            }
            set
            {
                _Heading = value;
            }
        }
        #endregion

        #region Private Fields
        private TrackType _Type;
        private double _Heading;
        private static char _SplitChar = ',';
        #endregion

        /// <summary>
        /// Public constructor with a value
        /// </summary>
        /// <param name="value"></param>
        public Track(string value) : base("Track")
        {
            if(value == null)
            {
                throw new Exceptions.WordFormatException("The value of the word must not be null",null);
            }

            if (_SplitChar != value[value.Length - 2])
            {
                throw new Exceptions.WordFormatException("The track word was not properly formatted", null);
            }

            try
            {
                if(value[value.Length-1] == 'M' || value[value.Length - 1] == 'm')
                {
                    _Type = TrackType.Magnetic;
                }
                else if (value[value.Length - 1] == 'T' || value[value.Length - 1] == 't')
                {
                    _Type = TrackType.True;
                }
                else
                {
                    _Type = TrackType.Unknown;
                }

                if(value.Length == 2)
                {
                    _Heading = double.NaN;
                    return;
                }
                if(double.TryParse(value.Substring(0,value.Length - 2),out _Heading))
                {
                    return;
                }
                else
                {
                    throw new Exceptions.WordFormatException("There was an error while parsing the value of the heading, value must be a number " 
                        + value.Substring(0, value.Length - 2) + " was supplied", null);
                }

            }
            catch (Exception excpt)
            {
                throw new Exceptions.WordFormatException("Error while parsing the track word, refer to the inner exception for more details",excpt);
            }   
        }

        
    }

    public enum TrackType
    {
        Magnetic,
        True,
        Unknown
    }
}
