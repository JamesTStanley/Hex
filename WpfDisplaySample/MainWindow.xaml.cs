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
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawAFlatTopHexGrid_Click(object sender, RoutedEventArgs e)
        {
            DrawHexGrid(CreateHexMapWithRingColors(HexOrientation.FlatTopped, 4));
        }

        private void DrawAPointyTopHexGrid_Click(object sender, RoutedEventArgs e)
        {
            DrawHexGrid(CreateHexMapWithRingColors(HexOrientation.PointyTopped, 4));
        }

        /// <summary>
        /// Draw the provided hex map on the form's canvas
        /// </summary>
        private void DrawHexGrid(HexMap<SolidColorBrush> hexGrid)
        {
            const double strokeThickness = 1;

            foreach (var hex in hexGrid.Map)
            {
                var hexPath = new Path
                {
                    Data = DrawHexagon(hex),
                    Fill = hex.Value,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = strokeThickness
                };
                hexPath.SetValue(Canvas.LeftProperty, hex.X - (hexPath.Width / 2));
                hexPath.SetValue(Canvas.TopProperty, hex.Y - (hexPath.Height / 2));
                CenterCanvas.Children.Add(hexPath);
            }
        }

        /// <summary>
        /// Use the coordinate data from a HexMapItem to produce a XAML PathGeometry
        /// </summary>
        private static PathGeometry DrawHexagon(HexMapItem<SolidColorBrush> hex)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure {StartPoint = new Point(hex.Vertices[0].Item1, hex.Vertices[0].Item2)};
            figure.Segments.Add(new LineSegment(new Point(hex.Vertices[1].Item1, hex.Vertices[1].Item2), true));
            figure.Segments.Add(new LineSegment(new Point(hex.Vertices[2].Item1, hex.Vertices[2].Item2), true));
            figure.Segments.Add(new LineSegment(new Point(hex.Vertices[3].Item1, hex.Vertices[3].Item2), true));
            figure.Segments.Add(new LineSegment(new Point(hex.Vertices[4].Item1, hex.Vertices[4].Item2), true));
            figure.Segments.Add(new LineSegment(new Point(hex.Vertices[5].Item1, hex.Vertices[5].Item2), true));
            figure.IsClosed = true;
            geometry.Figures.Add(figure);

            return geometry;
        }

        /// <summary>
        /// Create a HexMap of type SolidColorBrush and set the value to a different colored
        /// brush for each ring in the map
        /// </summary>
        /// <param name="orientation">Flat or pointy topped</param>
        /// <param name="ringCount">Number of rings around the center hex</param>
        private static HexMap<SolidColorBrush> CreateHexMapWithRingColors(HexOrientation orientation, int ringCount)
        {
            // Set up some colors for hex fills
            var ringColors = new[] 
            {
                new SolidColorBrush(Colors.Red), 
                new SolidColorBrush(Colors.Yellow),
                new SolidColorBrush(Colors.Blue),
                new SolidColorBrush(Colors.Orange),
                new SolidColorBrush(Colors.Green)
            };

            var hexGrid = new HexMap<SolidColorBrush>(orientation, ringCount, 45);

            // Set the value of each item to a solid color brush which varies per ring
            for (int i = 0; i <= Math.Min(ringCount, ringColors.Length - 1); i++)
                foreach (var hex in hexGrid.Ring(i))
                    hex.Value = ringColors[i];

            return hexGrid;
        }

    }
}
