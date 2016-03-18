using System;
using System.ComponentModel;
using NMEA_Tools.Decoder.Words;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Sentences
{
    /// <summary>
    ///  $GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48
    ///
    /// where:
    ///     VTG Track made good and ground speed
    ///     054.7,T     True track made good(degrees)
    ///     034.4,M     Magnetic track made good
    ///     005.5,N     Ground speed, knots
    ///     010.2,K     Ground speed, Kilometers per hour
    ///     *48         Checksum
    /// Note that, as of the 2.3 release of NMEA, there is a new field in the VTG sentence at the end just prior to the checksum.For more information on this field see here.
    ///
    /// Receivers that don't have a magnetic deviation (variation) table built in will null out the Magnetic track made good.
    /// </summary>
    public class GPVTG : Sentence, INotifyDataErrorInfo
    {
        public Track MagneticTrack { get; set; }
        public Track TrueTrack { get; set; }
        public GroundSpeed GroundSpeed { get; set; }

        public GPVTG(string value) : base(value)
        {
            if (value == null)
            {
                return;
            }

            string[] splitSentence = value.Split(Sentence._SentenceSplitChars);

            if (splitSentence.Length != 9)
            {
                throw new Exceptions.WordFormatException(String.Format(
                    "The GPVTG sentence does not contain the proper number of words {0} provided 10 expected: {1}",
                    splitSentence.Length, value));
            }

            if (splitSentence[0] != "$GPVTG")
            {
                throw new Exceptions.WordFormatException("The GPVTG sentence does not have the proper preamble + Data Type $GPVTG expected " + splitSentence[0] + " provided");
            }


            this.MagneticTrack = new Track(splitSentence[1]);
            this.TrueTrack = new Track(splitSentence[3]);
            this.GroundSpeed = new GroundSpeed(splitSentence[5]);

            this.Checksum = new Checksum(splitSentence[8]);
        }

        internal override bool SentenceHasErrors()
        {
            throw new NotImplementedException();
        }
    }
}
