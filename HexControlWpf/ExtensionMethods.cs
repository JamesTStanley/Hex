using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HexControlWpf
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Return a point collection of all the points in the List of Tuples
        /// for the boundary of the hexagon
        /// </summary>
        public static PointCollection AsBoundary(this List<Tuple<double, double>> verticies)
        {
            var pointCollection = new PointCollection(6);
            foreach (var vertex in verticies)
            {
                pointCollection.Add(vertex.AsPoint());
            }

            return pointCollection;
        }

        /// <summary>
        /// Return a point collection of the specified point in the List of Tuples
        /// and the next point, for a single face of the hexagon
        /// </summary>
        public static PointCollection AsFace(this List<Tuple<double, double>> verticies, int startingPoint)
        {
            var pointCollection = new PointCollection(2);

            var vertex1 = verticies[startingPoint];
            var vertex2 = startingPoint == 5 ? verticies[0] : verticies[startingPoint + 1];

            pointCollection.Add(vertex1.AsPoint());
            pointCollection.Add(vertex2.AsPoint());

            return pointCollection;
        }

        /// <summary>
        /// Return a point collection of the specified point in the List of Tuples
        /// and the next point and the derived center point, for a single sextant of the hexagon
        /// </summary>
        public static PointCollection AsSextant(this List<Tuple<double, double>> verticies, int startingPoint)
        {
            var pointCollection = new PointCollection(3);

            var centerX = verticies.Average(v => v.Item1);
            var centerY = verticies.Average(v => v.Item2);

            var vertex1 = verticies[startingPoint];
            var vertex2 = startingPoint == 5 ? verticies[0] : verticies[startingPoint + 1];

            pointCollection.Add(new Point(centerX, centerY));
            pointCollection.Add(vertex1.AsPoint());
            pointCollection.Add(vertex2.AsPoint());

            return pointCollection;
        }

        /// <summary>
        /// Return a Rect structure indicating the bounding rectangle for the vertex
        /// from the specified tuple and the next tuple.
        /// </summary>
        public static Rect AsSextantBoundingBox(this List<Tuple<double, double>> verticies, int startingPoint)
        {
            var centerX = verticies.Average(v => v.Item1);
            var centerY = verticies.Average(v => v.Item2);

            var point1X = verticies[startingPoint].Item1;
            var point1Y = verticies[startingPoint].Item2;
            var point2X = startingPoint == 5 ? verticies[0].Item1 : verticies[startingPoint + 1].Item1;
            var point2Y = startingPoint == 5 ? verticies[0].Item2 : verticies[startingPoint + 1].Item2;

            return new Rect(Math.Min(centerX, Math.Min(point1X, point2X)),
                Math.Min(centerY, Math.Min(point1Y, point2Y)),
                (Math.Max(centerX, Math.Max(point1X, point2X)) - Math.Min(centerX, Math.Min(point1X, point2X))),
                (Math.Max(centerY, Math.Max(point1Y, point2Y)) - Math.Min(centerY, Math.Min(point1Y, point2Y))));
        }

        /// <summary>
        /// Return a clip geometry for the specified point in the List of Tuples
        /// and the next point, for a single sextant of the hexagon using the sextant's
        /// local bounding box upper-left corner as the 0,0 coordinate location
        /// </summary>
        /// <param name="verticies"></param>
        /// <param name="startingPoint"></param>
        /// <returns></returns>
        public static Geometry AsSextantClipGeometry(this List<Tuple<double, double>> verticies, int startingPoint)
        {
            var centerX = verticies.Average(v => v.Item1);
            var centerY = verticies.Average(v => v.Item2);

            var vertex1 = verticies[startingPoint];
            var vertex2 = startingPoint == 5 ? verticies[0] : verticies[startingPoint + 1];

            var vertex1Point = new Point(vertex1.Item1 - centerX, vertex1.Item2 - centerY);
            var vertex2Point = new Point(vertex2.Item1 - centerX, vertex2.Item2 - centerY);
            
            var segmentCollection = new PathSegmentCollection(2);
            segmentCollection.Add(new LineSegment(vertex1Point, true));
            segmentCollection.Add(new LineSegment(vertex2Point, true));
            var figureCollection = new PathFigureCollection(1);
            figureCollection.Add(new PathFigure(new Point(0,0), segmentCollection, true));
            var clip = new PathGeometry(figureCollection);
            clip.FillRule = FillRule.Nonzero;
            return clip;
        }

        /// <summary>
        /// Return a Point for the specified tuple
        /// </summary>
        public static Point AsPoint(this Tuple<double, double> tuple)
        {
            return new Point(tuple.Item1, tuple.Item2);
        }
    }
}
