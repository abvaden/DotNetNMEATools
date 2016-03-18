using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NMEA_Tools.Decoder.Words;
using NMEA_Tools.Decoder;

namespace NMEA_Tools.Decoder.Sentences
{
    /// <summary>
    /// Example string
    /// $GPGLL,4916.45,N,12311.12,W,225444,A,*1D
    ///
    ///Where:
    ///   GLL Geographic position, Latitude and Longitude
    ///   4916.46,N Latitude 49 deg. 16.45 min.North
    ///   12311.12,W Longitude 123 deg. 11.12 min.West
    ///   225444       Fix taken at 22:54:44 UTC
    ///   A            Data Active or V(void)
    ///   * iD          checksum data
    /// </summary>
    public class GPGLL : Sentence, INotifyDataErrorInfo
    {
        public static Type[] Words = new Type[]
        {
            typeof(Latitude),
            typeof(Longitude),
            typeof(FixTime),
            typeof(ActiveState),
            typeof(Checksum)
        };

		public Latitude Latitude { get; set; }
		public Longitude Longitude { get; set; }
		public FixTime FixTime { get; set; }
		public ActiveState ActiveState { get; set; }


        public GPGLL(string value) : base(value)
        {
            if(value == null)
            {
                return;
            }

            string[] splitSentence = value.Split(Sentence._SentenceSplitChars);
            
            if(splitSentence.Length != 9)
            {
                throw new Exceptions.WordFormatException(String.Format(
                    "The GPGLL sentence does not contain the proper number of words {0} provided 8 expected: {1}",
                    splitSentence.Length,value));
            }

            if (splitSentence[0] != "$GPGLL")
            {
                throw new Exceptions.WordFormatException("The GPGLL sentence does not have the proper preamble + Data Type $GPGLL expected " + splitSentence[0] + " provided");
            }

			this.Longitude = new Longitude(splitSentence [1] + "," + splitSentence [2]);
			this.Latitude = new Latitude(splitSentence [3] + "," + splitSentence [4]);

			this.FixTime = new FixTime(splitSentence [5]);
			this.ActiveState = new ActiveState(splitSentence [6]);
			this.Checksum = new Checksum(splitSentence [7]);
        }

        internal override bool SentenceHasErrors()
        {
            throw new NotImplementedException();
        }
    }
}
