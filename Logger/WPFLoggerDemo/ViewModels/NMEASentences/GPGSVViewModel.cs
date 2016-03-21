using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using NMEA_Tools.Serial;
using NMEA_Tools.Decoder.Sentences;

namespace WPFLoggerDemo.ViewModels.NMEASentences
{
    public class GPGSVViewModel : DataViewModelBase
    {
        public int NumberSatellites { get; private set; }
        public List<SatelliteData> SatellitesInView { get; private set; }

        private GPGSV _GPGSV;
        private List<int> _ReceivedMessages;

        public GPGSVViewModel(Listener listener) : base(listener, "GPGSV Words")
        {
            _ReceivedMessages = new List<int>();
            SatellitesInView = new List<SatelliteData>();
        }

        public override void SentenceReceived(string sentence)
        {
            if(sentence.Contains("GPGSV"))
            {
                try
                {
                    _GPGSV = new GPGSV(sentence);

                    if (_ReceivedMessages.Count == 0)
                    {
                        for(int i = 0; i < _GPGSV.NumberMessages; i++)
                        {
                            _ReceivedMessages.Add(i);
                            SatellitesInView.Clear();
                        }
                    }

                    _ReceivedMessages.Remove(_GPGSV.MessageNumber);


                    for(int i = 0; i < _GPGSV.SatellitePRN.Length; i++)
                    {
                        SatellitesInView.Add(new SatelliteData(
                            _GPGSV.SatellitePRN[i],
                            _GPGSV.SatelliteElevation[i],
                            _GPGSV.SatelliteAzimuth[i],
                            _GPGSV.SatelliteSNR[i]
                            ));
                    }

                    if(_ReceivedMessages.Count == 0)
                    {
                        NumberSatellites = SatellitesInView.Count;
                        RaisePropertyChanged("NumberSatellites");
                        RaisePropertyChanged("SatellitesInView");
                    }
                }
                catch (Exception excpt)
                {
                    System.Diagnostics.Debug.WriteLine(excpt.Message);
                }
            }
        }

        public class SatelliteData
        {
            public string PRN { get; }
            public string Elevation { get; }
            public string Azimuth { get; }
            public string SNR { get; }

            public SatelliteData(double prn, double elevation, double azimuth, double snr)
            {
                PRN = prn.ToString();
                Elevation = elevation.ToString();
                Azimuth = azimuth.ToString();
                SNR = snr.ToString();
            }
        }
    }
}
