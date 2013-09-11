using System;
using System.Collections.Generic;

namespace Hex
{
    /// <summary>
    /// This extends HexMapItemBase by adding properties that are necessary
    /// for making hex maps displayable. The constructor requires an orientation
    /// for the map and a size for the hex, from which the rest of the
    /// properties can be calculated.
    /// </summary>
    public class HexMapItem<T> : HexMapItemBase<T>
    {
        #region Constructors

        public HexMapItem(HexOrientation orientation, double size, int x, int y, int z)
            : base(x, y, z)
        {
            Orientation = orientation;
            Size = size;
            CalculateCetnerPoint();
            CalculateVerticies();
            DeriveFacesFromVerticies();
        }

        public HexMapItem(HexOrientation orientation, double size, int x, int y, int z, T value)
            : this(orientation, size, x, y, z)
        {
            Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The rendering orientation for the hex
        /// </summary>
        public HexOrientation Orientation { get; private set; }
        
        /// <summary>
        /// The distance from the center point to a vertice
        /// </summary>
        public double Size { get; private set; }
        
        /// <summary>
        /// The X,Y coordinates of the center point of the hex
        /// </summary>
        public Tuple<double, double> CenterPoint { get; private set; }
        
        /// <summary>
        /// The list of X,Y coordinates of the six verticies of the hex
        /// </summary>
        public List<Tuple<double, double>> Vertices { get; private set; }

        /// <summary>
        /// The list of start and end X,Y coordinates of the size faces of the hex
        /// </summary>
        public List<Tuple<Tuple<double, double>, Tuple<double, double>>> Faces { get; private set; }

        #endregion

        #region Calulate/Derive Public Properties

        /// <summary>
        /// Calculate the center point of the hex based on the orientation
        /// and size
        /// </summary>
        private void CalculateCetnerPoint()
        {
            double centerX;
            double centerY;
            if (Orientation == HexOrientation.FlatTopped)
            {
                centerX = Size * 3 / 2 * Q;
                centerY = Size * Math.Sqrt(3) * (R + 0.5 * Q);
            }
            else
            {
                centerX = Size * Math.Sqrt(3) * (Q + 0.5 * R);
                centerY = Size * 3 / 2 * R;
            }

            CenterPoint = new Tuple<double, double>(centerX,centerY);
        }

        /// <summary>
        /// Calculate the location of the six verticies of the hex
        /// based on the center point, orientation and size.
        /// </summary>
        private void CalculateVerticies()
        {
            var x = CenterPoint.Item1;
            var y = CenterPoint.Item2;

            Vertices = new List<Tuple<double, double>>(6);

            for (int i = 0; i < 6; i++)
            {
                double angle;
                if (Orientation == HexOrientation.FlatTopped)
                    angle = 2 * Math.PI / 6 * i;
                else
                    angle = 2 * Math.PI / 6 * (i + 0.5);

                var verticeX = x + Size * Math.Cos(angle);
                var verticeY = y + Size * Math.Sin(angle);

                Vertices.Add(new Tuple<double, double>(verticeX,verticeY));
            }
        }

        /// <summary>
        /// Derive the start/end coordinates of the six faces of the
        /// hex by stringing together the pairs of vertices that are
        /// the endpoints of each face.
        /// </summary>
        private void DeriveFacesFromVerticies()
        {
            Faces = new List<Tuple<Tuple<double, double>, Tuple<double, double>>>
                {
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(Vertices[0], Vertices[1]),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(Vertices[1], Vertices[2]),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(Vertices[2], Vertices[3]),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(Vertices[3], Vertices[4]),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(Vertices[4], Vertices[5]),
                    new Tuple<Tuple<double, double>, Tuple<double, double>>(Vertices[5], Vertices[0])
                };
        }

        #endregion
    }
}
