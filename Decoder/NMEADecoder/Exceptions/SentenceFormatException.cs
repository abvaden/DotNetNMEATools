using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Exceptions
{
    public class SentenceFormatException : Exception
    {
        public SentenceFormatException(string message, Exception innerException) : base(message,innerException)
        {
        }
    }
}
