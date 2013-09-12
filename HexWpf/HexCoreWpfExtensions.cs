using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Hex;

namespace HexWpf
{
    /// <summary>
    /// Extension methods for the HexMapItem class that return
    /// coordinates, lines, areas, etc. as WPF native structures
    /// and objects instead of generic Tuples that need to be
    /// converted. Additionally, it takes care of multiplying Y values
    /// by -1 since the XAML coordinate system has 0,0 in the upper
    /// left with values increasing down and to the right whereas the 
    /// HexMap/Item classes use a Cartesian coordinate system.
    /// </summary>
    public static class HexCoreWpfExtensions
    {
        /// <summary>
        /// Returns the center point of the hex
        /// </summary>
        /// <returns>A WPF Point struct</returns>
        public static Point CenterPoint(this HexMapItem item )
        {
            return new Point(item.CenterPoint.Item1, -1 * item.CenterPoint.Item2);
        }

        public static List<Point> Verticies(this HexMapItem item)
        {
            var verticies = new List<Point>(6);
            verticies.AddRange(item.Vertices.Select(vertex => new Point(vertex.Item1, -1 * vertex.Item2)));

            return verticies;
        }

        public static Point Verticies(this HexMapItem item, int index)
        {
            return new Point(item.Vertices[index].Item1, -1 * item.Vertices[index].Item2);
        }

        public static Line Faces(this HexMapItem item, int index)
        {
            var line = new Line
                {
                    X1 = item.Faces[index].Item1.Item1,
                    Y1 = -1 * item.Faces[index].Item1.Item2,
                    X2 = item.Faces[index].Item2.Item1,
                    Y2 = -1 * item.Faces[index].Item2.Item2
                };
            return line;
        }

        public static PathGeometry PathGeometry(this HexMapItem item)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure { StartPoint = item.Verticies(0) };
            figure.Segments.Add(new LineSegment(item.Verticies(1), true));
            figure.Segments.Add(new LineSegment(item.Verticies(2), true));
            figure.Segments.Add(new LineSegment(item.Verticies(3), true));
            figure.Segments.Add(new LineSegment(item.Verticies(4), true));
            figure.Segments.Add(new LineSegment(item.Verticies(5), true));
            figure.IsClosed = true;
            geometry.Figures.Add(figure);

            return geometry;
        }
    }
}
