using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Serial
{
    public class ListenerException : Exception
    {
        public ListenerException(string message, Exception innerException) : 
            base(message, innerException)
        {
        }
    }
}
