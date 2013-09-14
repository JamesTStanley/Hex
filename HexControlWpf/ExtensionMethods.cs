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
        /// </summary>
        public static PointCollection AsPointCollection(this List<Tuple<double, double>> verticies)
        {
            var pointCollection = new PointCollection(6);
            foreach (var point in verticies)
            {
                pointCollection.Add(new Point(point.Item1, point.Item2));
            }

            return pointCollection;
        }

        /// <summary>
        /// Return a point collection of the specified point in the List of Tuples
        /// and the next point.
        /// </summary>
        public static PointCollection AsPointCollection(this List<Tuple<double, double>> verticies, int startingPoint)
        {
            var pointCollection = new PointCollection(2);

            var point1 = verticies[startingPoint];
            var point2 = startingPoint == 5 ? verticies[0] : verticies[startingPoint + 1];

            pointCollection.Add(new Point(point1.Item1, point1.Item2));
            pointCollection.Add(new Point(point2.Item1, point2.Item2));

            return pointCollection;
        }
    }
}
