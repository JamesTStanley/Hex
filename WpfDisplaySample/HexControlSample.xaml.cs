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
using System.Windows.Shapes;

namespace WpfDisplaySample
{
    /// <summary>
    /// Interaction logic for HexControlSample.xaml
    /// </summary>
    public partial class HexControlSample : Window
    {
        public HexControlSample()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SampleHex.HexFaces[0].Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
