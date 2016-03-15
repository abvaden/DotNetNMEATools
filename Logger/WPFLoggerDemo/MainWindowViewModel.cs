using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFLoggerDemo
{
    public class MainWindowViewModel
    {
        public ViewModels.SerialPortListenerSetupViewModel SerialPortSetupViewModel { get; set; }

        public MainWindowViewModel()
        {
            SerialPortSetupViewModel = new ViewModels.SerialPortListenerSetupViewModel();
        }
    }
}
