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
using Hex;

namespace WpfDisplaySample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawAHexGrid_Click(object sender, RoutedEventArgs e)
        {
            var hexGrid = new Hex.HexMap<object>(MapShape.HexagonFlatTopped, 1);

            foreach (var hex in hexGrid.Map)
            {
                const double size = 30;

                var x = size*3/2*hex.Q;
                var y = size*Math.Sqrt(3)*(hex.R + hex.Q/2);

                var z = new Path();
                z.Data = DrawHexagon(x, y, size, 2);
                z.Stroke = new SolidColorBrush(Colors.Black);
                z.StrokeThickness = 2;
                z.SetValue(Canvas.LeftProperty, x);
                z.SetValue(Canvas.TopProperty, y);
                CenterCanvas.Children.Add(z);
            }
        }

        private PathGeometry DrawHexagon(double x, double y, double size, double strokeThickness)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure();

            for (int i = 0; i < 6; i++)
            {
                var angle = 2*Math.PI/6*i;
                var x_i = x + size*Math.Cos(angle);
                var y_i = y + size*Math.Sin(angle);
                if (i == 0)
                    figure.StartPoint = new Point(x_i, y_i);
                else
                {
                    LineSegment segment = new LineSegment();
                    segment.Point = new Point(x_i + 0.5*strokeThickness, y_i + 0.5*strokeThickness);
                    figure.Segments.Add(segment);
                }
            }
            figure.IsClosed = true;
            geometry.Figures.Add(figure);

            return geometry;
        }
    }
}
