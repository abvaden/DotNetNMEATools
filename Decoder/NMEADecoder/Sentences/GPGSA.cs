using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Sentences
{
    /// <summary>
    /// GPGSA : GPS DOP and active satellites
    /// </summary>
    /// <remarks>
    /// eg1. $GPGSA,A,3,,,,,,16,18,,22,24,,,3.6,2.1,2.2*3C
    /// eg2. $GPGSA,A,3,19,28,14,18,27,22,31,39,,,,,1.7,1.0,1.3*35
    ///
    ///
    /// 1    = Mode:
    /// M=Manual, forced to operate in 2D or 3D
    /// A=Automatic, 3D/2D
    /// 2    = Mode:
    /// 1=Fix not available
    /// 2=2D
    /// 3=3D
    /// 3-14 = IDs of SVs used in position fix(null for unused fields)
    /// 15   = PDOP
    /// 16   = HDOP
    /// 17   = VDOP
    /// </remarks>
    public class GPGSA : Sentence
    {
        #region Publid Properties
        public GSAMode Mode
        {
            get
            {
                return _Mode;
            }
            set
            {
                _Mode = value;
            }
        }
        public GSADimension Dimension
        {
            get
            {
                return _Dimension;
            }
            set
            {
                _Dimension = value;
            }
        }
        public int[] SatIDs
        {
            get
            {
                if (_SatIDs != null)
                {
                    return _SatIDs.ToArray();
                }
                return new int[0];
            }
            set
            {
                _SatIDs = value.ToList();
            }
        }
        public double HDOP
        {
            get
            {
                return _HDOP;
            }
            set
            {
                _HDOP = value;
            }
        }
        public double PDOP
        {
            get
            {
                return _PDOP;
            }
            set
            {
                _PDOP = value;
            }
        }
        public double VDOP
        {
            get
            {
                return _VDOP;
            }
            set
            {
                _VDOP = value;
            }
        }
        #endregion

        #region Private Fields
        private GSADimension _Dimension;
        private GSAMode _Mode;
        private List<int> _SatIDs;
        private double _HDOP;
        private double _PDOP;
        private double _VDOP;
        #endregion

        public GPGSA(string value) : base(value)
        {
            #region Error checking on the input value
            if (value == null)
            {
                throw new Exceptions.SentenceFormatException("The value of the sentence must not be null", null);
            }

            string[] splitSentence = value.Split(Sentence._SentenceSplitChars);

            if(splitSentence[0] != "$GPGSA")
            {
                throw new Exceptions.SentenceFormatException("The preamble of the sentence is incorrect expected GPGSA, " 
                    + splitSentence[0] + " was given", null);
            }
            if(splitSentence.Length != 19)
            {
                throw new Exceptions.SentenceFormatException(String.Format(
                    "The sentence does not have the proper number of words 18 expected {0} received", splitSentence.Length), null);
            }
            #endregion

            #region Mode 
            if(splitSentence[1].ToUpper() == "M" )
            {
                _Mode = GSAMode.Manual;
            }
            else if(splitSentence[1].ToUpper() == "A")
            {
                _Mode = GSAMode.Automatic;
            }
            else
            {
                throw new Exceptions.WordFormatException("The value given for mode is invalid, expected a/A or m/M received " + splitSentence[1],null);
            }
            #endregion

            #region Dimension
            if (splitSentence[2] == "3")
            {
                _Dimension = GSADimension.Three;
            }
            else if (splitSentence[2]== "2")
            {
                _Dimension = GSADimension.Two;
            }
            else
            {
                _Dimension = GSADimension.Unknown;
            }
            #endregion

            #region Satalites
            _SatIDs = new List<int>();
            int satID = 0;
            for(int i = 3; i < 14; i++)
            {
                if(!String.IsNullOrEmpty(splitSentence[i]))
                {
                    if(int.TryParse(splitSentence[i],out satID))
                    {
                        _SatIDs.Add(satID);
                    }
                    else
                    {
                        throw new Exceptions.WordFormatException("The id for the satalite was invalid, a number was expected " 
                            + splitSentence[i] + " was given",null);
                    }
                }
            }
            #endregion

            #region DOP
            double.TryParse(splitSentence[16], out _HDOP);
            double.TryParse(splitSentence[15], out _PDOP);
            double.TryParse(splitSentence[17], out _VDOP);
            #endregion
        }

        internal override bool SentenceHasErrors()
        {
            throw new NotImplementedException();
        }

        
    }

    public enum GSAMode { Manual, Automatic }
    public enum GSADimension { Two, Three, Unknown }
}
