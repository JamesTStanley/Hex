using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            var hexGrid = CreateHexMapWithRingColors();

            foreach (var hex in hexGrid.Map)
            {
                // The hex map itself isn't concerned with the size of a hex,
                // so we need to invent one. This is the "radius" or more
                // appropriately the distance from the center to a vertex point
                const double size = 45;
               
                const double strokeThickness = 1;

                // Calculate the X,Y location of a hex for a plane with 0,0
                // in the center. Because the Canvas panel in XAML has no clipping
                // region by default, we can place a Canvas in the lower-right quadrant
                // of our drawing space and use its upper-left corner as 0,0 to
                // achieve the desired coordinate system
                var centerX = size * 3 / 2 * hex.Q;
                var centerY = size * Math.Sqrt(3) * (hex.R + 0.5 * (hex.Q ));

                var hexPath = new Path
                    {
                        Data = DrawHexagon(centerX, centerY, size, strokeThickness),
                        Fill = hex.Value,
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = strokeThickness
                    };
                hexPath.SetValue(Canvas.LeftProperty, centerX - (hexPath.Width / 2));
                hexPath.SetValue(Canvas.TopProperty, centerY - (hexPath.Height / 2));
                CenterCanvas.Children.Add(hexPath);
            }
        }

        private PathGeometry DrawHexagon(double x, double y, double size, double strokeThickness)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure();

            for (int i = 0; i < 6; i++)
            {
                var angle = 2 * Math.PI / 6 * i;
                var verticeX = x + size * Math.Cos(angle);
                var verticeY = y + size * Math.Sin(angle);
                if (i == 0)
                    figure.StartPoint = new Point(verticeX, verticeY);
                else
                {
                    var segment = new LineSegment
                        {
                            Point = new Point(verticeX + 0.5*strokeThickness, verticeY + 0.5*strokeThickness)
                        };
                    figure.Segments.Add(segment);
                }
            }
            figure.IsClosed = true;
            geometry.Figures.Add(figure);

            return geometry;
        }

        private HexMap<SolidColorBrush> CreateHexMapWithRingColors()
        {
            var hexGrid = new HexMap<SolidColorBrush>(MapShape.HexagonFlatTopped, 4);
            
            // Set the value of each item to a solid color brush which varies per ring
            foreach (var hex in hexGrid.Ring(0))
            {
                hex.Value = new SolidColorBrush(Colors.Red);
            }
            foreach (var hex in hexGrid.Ring(1))
            {
                hex.Value = new SolidColorBrush(Colors.Yellow);
            }
            foreach (var hex in hexGrid.Ring(2))
            {
                hex.Value = new SolidColorBrush(Colors.Blue);
            }
            foreach (var hex in hexGrid.Ring(3))
            {
                hex.Value = new SolidColorBrush(Colors.Orange);
            }
            foreach (var hex in hexGrid.Ring(4))
            {
                hex.Value = new SolidColorBrush(Colors.Green);
            }

            return hexGrid;
        }

        
    }
}
