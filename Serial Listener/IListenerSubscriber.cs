using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Serial
{
    public interface IListenerSubscriber
    {
        void SentenceReceived();
    }
}
