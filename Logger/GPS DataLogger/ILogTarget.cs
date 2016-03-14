using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.GPS_DataLogger
{
    public interface ILogTarget
    {
        void OpenLog();
        void CloseLog();
        void Log();
    }
}
