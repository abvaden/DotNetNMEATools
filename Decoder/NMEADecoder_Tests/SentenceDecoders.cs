using System;
using NUnit.Framework;
using NMEA_Tools.Decoder.Sentences;

namespace NeamDecoder_Tests
{
	[TestFixture]
	public class SentenceDecoding
	{
		[Test]
		public void GPGLLDecoding()
		{
			string[] gpgllSentences = new string[]
			{
				"$GPGLL,3525.2462,N,08051.9551,W,173744.942,V,N*59",
				"$GPGLL,3526.5412,N,08052.5137,W,182648.769,A,A*4D",
				"$GPGLL,,,,,000037.581,V,N*72"
			};

			for(int i = 0; i < gpgllSentences.Length; i++)
			{
				GPGLL gpgll = new GPGLL(gpgllSentences[i]);
				Assert.IsNotNull (gpgll, "The parsing of a $GPGLL string failed with input " + gpgllSentences [i]);
                Assert.IsNotNull(gpgll.Longitude);
                Assert.IsNotNull(gpgll.Latitude);
                Assert.IsNotNull(gpgll.FixTime);
                Assert.IsNotNull(gpgll.ActiveState);
			}
		}

        [Test]
        public void GPVTGDecoding()
        {
            string[] gppgtvSentences = new string[]
            {
                "$GPVTG,,T,,M,,N,,K*48",
                "$GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48",
                "$GPVTG,360.0,T,348.7,M,000.0,N,000.0,K*43"
            };

            for(int i =0; i < gppgtvSentences.Length; i++)
            {
                GPVTG gpvtg = new GPVTG(gppgtvSentences[i]);
                Assert.IsNotNull(gpvtg.MagneticTrack);
                Assert.IsNotNull(gpvtg.TrueTrack);
                Assert.IsNotNull(gpvtg.GroundSpeed);
            }
        }
        
        [Test]
        public void GPGSADecoding()
        {
            string[] gpgsaSentences = new string[]
            {
                "$GPGSA,A,3,,,,,,16,18,,22,24,,,3.6,2.1,2.2*3C",
                "$GPGSA,A,3,19,28,14,18,27,22,31,39,,,,,1.7,1.0,1.3*35"
            };

            for (int i = 0; i < gpgsaSentences.Length; i++)
            {
                GPGSA gpgsa = new GPGSA(gpgsaSentences[i]);
                Assert.IsNotNull(gpgsa.Dimension);
                Assert.IsNotNull(gpgsa.Mode);
                Assert.IsNotNull(gpgsa.HDOP);
                Assert.IsNotNull(gpgsa.VDOP);
                Assert.IsNotNull(gpgsa.PDOP);
                Assert.IsNotNull(gpgsa.SatIDs);
            }
        }
	}
}
