using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.GPS_DataLogger
{
    public class LoggerException : Exception
    {
        public LoggerException(string message, Exception internalException) : base(message,internalException)
        { }
    }
}
