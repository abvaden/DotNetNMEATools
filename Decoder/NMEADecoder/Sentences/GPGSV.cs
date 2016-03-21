using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Decoder.Words;

namespace NMEA_Tools.Decoder.Sentences
{
    public class GPGSV : Sentence
    {
        #region Public Properties
        public int NumberMessages
        {
            get
            {
                return _NumberMessages;
            }
            set
            {
                _NumberMessages = value;
            }
        }
        public int MessageNumber
        {
            get
            {
                return _MessageNumber;
            }
            set
            {
                _MessageNumber = value;
            }
        }
        public int SatellitesInView
        {
            get
            {
                return _SatellitesInView;
            }
            set
            {
                _SatellitesInView = value;
            }
        }

        public int[] SatellitePRN
        {
            get
            {
                return _SatellitePRN.ToArray();
            }
            set
            {
                _SatellitePRN = value.ToList();
            }
        }
        public double[] SatelliteElevation
        {
            get
            {
                return _SatelliteElevation.ToArray();
            }
            set
            {
                _SatelliteElevation = value.ToList();
            }
        }
        public double[] SatelliteAzimuth
        {
            get
            {
                return _SatelliteAzimuth.ToArray();
            }
            set
            {
                _SatelliteAzimuth = value.ToList();
            }
        }
        public double[] SatelliteSNR
        {
            get
            {
                return _SatelliteSNR.ToArray();
            }
            set
            {
                _SatelliteSNR = value.ToList();
            }
        }
        #endregion

        #region Private Fields
        private int _NumberMessages;
        private int _MessageNumber;
        private int _SatellitesInView;

        private List<int> _SatellitePRN;
        private List<double> _SatelliteElevation; 
        private List<double> _SatelliteAzimuth;
        private List<double> _SatelliteSNR;
        #endregion

        public GPGSV(string value) : base(value)
        {
            #region Input Validation
            if(String.IsNullOrEmpty(value))
            {
                throw new Exceptions.SentenceFormatException("The value of a GSV sentence must not be null", null);
            }

            string[] splitSentence = value.Split(Sentence._SentenceSplitChars);

            if(splitSentence.Length <  5 )
            {
                throw new Exceptions.SentenceFormatException("The value for a GSV sentence must have > 3 words, "
                    + (splitSentence.Length - 2).ToString() + " provided", null);
            }
            #endregion

            int.TryParse(splitSentence[1], out _NumberMessages);
            int.TryParse(splitSentence[2], out _MessageNumber);
            int.TryParse(splitSentence[3], out _SatellitesInView);

            #region Satellites
            _SatellitePRN = new List<int>();
            _SatelliteElevation = new List<double>();
            _SatelliteAzimuth = new List<double>();
            _SatelliteSNR = new List<double>();

            if((splitSentence.Length - 5) % 4 != 0)
            {
                throw new Exceptions.SentenceFormatException("GSV sentence format error, improper number of words", null);
            }

            int satPRN;
            double satElevation, satAzimuth, satSNR;
            for(int i = 4; i < splitSentence.Length - 1; i = i + 4)
            {
                #region PRN
                if(int.TryParse(splitSentence[i], out satPRN))
                {
                    _SatellitePRN.Add(satPRN);
                }
                else
                {
                    _SatellitePRN.Add(0);
                }
                #endregion

                #region Elevation
                if(double.TryParse(splitSentence[i+1], out satElevation))
                {
                    _SatelliteElevation.Add(satElevation);
                }
                else
                {
                    _SatelliteElevation.Add(0);
                }
                #endregion

                #region Azimuth
                if(double.TryParse(splitSentence[i+2],out satAzimuth))
                {
                    _SatelliteAzimuth.Add(satAzimuth);
                }
                else
                {
                    _SatelliteAzimuth.Add(0);
                }
                #endregion

                #region SNR
                if (double.TryParse(splitSentence[i+3],out satSNR))
                {
                    _SatelliteSNR.Add(satSNR);
                }
                else
                {
                    _SatelliteSNR.Add(0);
                }
                #endregion
            }
            #endregion 

            Checksum = new Checksum(splitSentence[splitSentence.Length - 1]);
        }

        internal override bool SentenceHasErrors()
        {
            throw new NotImplementedException();
        }
    }
}
