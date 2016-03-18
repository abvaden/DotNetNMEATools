using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEA_Tools.Serial;
using System.Collections.ObjectModel;
using WPFLoggerDemo.ViewModels.NMEASentences;

namespace WPFLoggerDemo
{
    public class MainWindowViewModel : ViewModels.ViewModelBase
    {
        public ViewModels.SerialPortListenerSetupViewModel SerialPortSetupViewModel { get; set; }
        public ViewModels.RawPortDataViewModel RawPortDataViewModel { get; set; }
        public ViewModels.NMEASentences.MapViewModel MapViewModel { get; set; }
        public ViewModels.NMEASentences.DataViewModelBase DataView { get; }
        public ObservableCollection<ViewModels.NMEASentences.DataViewModelBase> AvailableViewModels { get; }
        public ViewModels.NMEASentences.DataViewModelBase SelectedDataView
        {
            get
            {
                return _SelectedDataView;
            }
            set
            {
                if (_SelectedDataView != null)
                {
                    _SelectedDataView.Dispose();
                }
                _SelectedDataView = value;
                _SelectedDataView.Enable();
                RaisePropertyChanged();
            }
        }


        private Listener _Listerner;
        private ViewModels.NMEASentences.DataViewModelBase _SelectedDataView;

        public MainWindowViewModel()
        {
            _Listerner = new Listener();
            SerialPortSetupViewModel = new ViewModels.SerialPortListenerSetupViewModel(_Listerner);
            RawPortDataViewModel = new ViewModels.RawPortDataViewModel(_Listerner);
            MapViewModel = new ViewModels.NMEASentences.MapViewModel(_Listerner);


            DataView = new ViewModels.NMEASentences.DataViewModelBase(_Listerner, null);

            AvailableViewModels = new ObservableCollection<DataViewModelBase>();
            AvailableViewModels.Add(new MapViewModel(_Listerner));
            AvailableViewModels.Add(new ViewModels.NMEASentences.GPGLLWordsViewModel(_Listerner));
        }
    }
}
