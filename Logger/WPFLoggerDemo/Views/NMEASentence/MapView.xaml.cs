using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFLoggerDemo.Views.NMEASentence
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private System.Threading.Timer _Timer;

        public MapView()
        {

            InitializeComponent();
            _Timer = new System.Threading.Timer(new System.Threading.TimerCallback(_UpdateMap), null, 3000, 3000);    
        }

        private void _UpdateMap(object stateInfo)
        {
            string key = "AIzaSyC3xLwB_Z7chKbaSvWzqZxljXGa-TGo8IM";
            string browserWidth = Browser.Width.ToString();
            string browserHeight = Browser.Height.ToString();
            string html = string.Format("<iframe width=\"{0}\" height=\"{1}\" frameborder=\"0\" style =\"border:0\" " +
                          "src=\"https://www.google.com/maps/embed/v1/place?q={2},{3}&key={4}\" allowfullscreen ></iframe >",
                          browserWidth,
                          browserHeight,
                          LongtidueLabel.Content,
                          LatitudeLabel.Content,
                          key);

            Browser.NavigateToString(html);
        }

    }
}