﻿using System;
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
            var centerPoint = new Point(verticies.Average(v => v.Item1), verticies.Average(v => v.Item2));
            var vertex1 = verticies[startingPoint].AsPoint();
            var vertex2 = startingPoint == 5 ? verticies[0].AsPoint() : verticies[startingPoint + 1].AsPoint();
            
            // Adjust everything to a coordinate system where the upper left
            // bound of this sextant is 0,0
            var deltaX = Math.Min(centerPoint.X, Math.Min(vertex1.X, vertex2.X));
            var deltaY = Math.Min(centerPoint.Y, Math.Min(vertex1.Y, vertex2.Y));
            centerPoint.X -= deltaX;
            vertex1.X -= deltaX;
            vertex2.X -= deltaX;
            centerPoint.Y -= deltaY;
            vertex1.Y -= deltaY;
            vertex2.Y -= deltaY;

            var segmentCollection = new PathSegmentCollection(2) 
                {
                    new LineSegment(vertex1, true),
                    new LineSegment(vertex2, true)
                };

            var figureCollection = new PathFigureCollection(1)
                {
                    new PathFigure(centerPoint, segmentCollection, true)
                };

            var clip = new PathGeometry(figureCollection) { FillRule = FillRule.Nonzero };

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
