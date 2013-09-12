using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Hex;
using HexWpf;

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

        private void DrawAFlatTopHexGridWithVertAnnot_Click(object sender, RoutedEventArgs e)
        {
            var hexGrid = CreateHexMapWithRingColors(HexOrientation.FlatTopped, 4);
            DrawHexGrid(hexGrid);
            AnnotateVerticies(hexGrid);
        }

        private void DrawAPointyTopHexGridWithVertAnnot_Click(object sender, RoutedEventArgs e)
        {
            var hexGrid = CreateHexMapWithRingColors(HexOrientation.PointyTopped, 4);
            DrawHexGrid(hexGrid);
            AnnotateVerticies(hexGrid);
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
                    Data = hex.PathGeometry(),
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

        /// <summary>
        /// Draw textblock annotations near each verticie indicating its direction from center
        /// </summary>
        private void AnnotateVerticies(HexMap<SolidColorBrush> hexGrid)
        {
            foreach (var hex in hexGrid.Map)
            {
                for (int i = 0; i <= 5; i++ )
                {
                    var vertex = hex.Verticies(i);
                    var direction = hex.VerticeDirections[i];

                    var textBlock = new TextBlock {Text = direction.ToString()};

                    // Force some calculations of how big the textblock wants
                    // to be given the string that was placed in it
                    textBlock.Measure(new Size(double.PositiveInfinity,double.PositiveInfinity));
                    var width = textBlock.DesiredSize.Width;
                    var height = textBlock.DesiredSize.Height;

                    switch (direction)
                    {
                        case HexDirection.W:
                            textBlock.SetValue(Canvas.LeftProperty, vertex.X + 5);
                            break;
                        case HexDirection.SW:
                        case HexDirection.NW:
                            if (hex.Orientation == HexOrientation.FlatTopped)
                                textBlock.SetValue(Canvas.LeftProperty, vertex.X);
                            else
                                textBlock.SetValue(Canvas.LeftProperty, vertex.X + 5);
                            break;
                        case HexDirection.E:
                            textBlock.SetValue(Canvas.LeftProperty, vertex.X - width - 5);
                            break;
                        case HexDirection.SE:
                        case HexDirection.NE:
                            if (hex.Orientation == HexOrientation.FlatTopped)
                                textBlock.SetValue(Canvas.LeftProperty, vertex.X - width );
                            else
                                textBlock.SetValue(Canvas.LeftProperty, vertex.X - width - 5);
                            break;
                        case HexDirection.N:
                        case HexDirection.S:
                            textBlock.SetValue(Canvas.LeftProperty, vertex.X - width / 2);
                            break;
                    }

                    switch (direction)
                    {
                        case HexDirection.SE:
                        case HexDirection.SW:
                        case HexDirection.S:
                            textBlock.SetValue(Canvas.TopProperty, vertex.Y - height);
                            break;
                        case HexDirection.E:
                        case HexDirection.W:
                            textBlock.SetValue(Canvas.TopProperty, vertex.Y - height / 2);
                            break;
                        case HexDirection.NE:
                        case HexDirection.NW:
                        case HexDirection.N:                       
                            textBlock.SetValue(Canvas.TopProperty, vertex.Y);
                            break;
                    }

                    CenterCanvas.Children.Add(textBlock);
                }
            }
        }

        

    }
}
