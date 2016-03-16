using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Serial;

namespace WPFLoggerDemo
{
    public class MainWindowViewModel
    {
        public ViewModels.SerialPortListenerSetupViewModel SerialPortSetupViewModel { get; set; }
        public ViewModels.RawPortDataViewModel RawPortDataViewModel { get; set; }
        public ViewModels.NMEASentences.MapViewModel MapViewModel { get; set; }

        private Listener _Listerner;

        public MainWindowViewModel()
        {
            _Listerner = new Listener();
            SerialPortSetupViewModel = new ViewModels.SerialPortListenerSetupViewModel(_Listerner);
            RawPortDataViewModel = new ViewModels.RawPortDataViewModel(_Listerner);
            MapViewModel = new ViewModels.NMEASentences.MapViewModel(_Listerner);
        }
    }
}
