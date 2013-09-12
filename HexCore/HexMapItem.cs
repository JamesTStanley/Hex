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
    public class HexMapItem : HexMapItemBase
    {
        #region Constructors

        public HexMapItem(HexOrientation orientation, double size, int x, int y, int z)
            : base(x, y, z)
        {
            Orientation = orientation;
            Size = size;
            CalculateCetnerPoint();
            CalculateVerticies();
            SetVerticieDirections();
            DeriveFacesFromVerticies();
            SetFaceDirections();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The rendering orientation for the hex
        /// </summary>
        public HexOrientation Orientation { get; private set; }
        
        /// <summary>
        /// The distance from the center point to a vertex
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
        /// List of which cardinal direction the corresponding member of
        /// Verticies is from the center point
        /// </summary>
        public List<HexDirection> VerticeDirections { get; private set; } 

        /// <summary>
        /// The list of start and end X,Y coordinates of the size faces of the hex
        /// </summary>
        public List<Tuple<Tuple<double, double>, Tuple<double, double>>> Faces { get; private set; }

        /// <summary>
        /// List of which cardinal direction the corresponding member of
        /// Faces is from the center point
        /// </summary>
        public List<HexDirection> FaceDirections { get; private set; } 

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

                var vertexX = x + Size * Math.Cos(angle);
                var vertexY = y + Size * Math.Sin(angle);

                Vertices.Add(new Tuple<double, double>(vertexX, vertexY));
            }
        }

        /// <summary>
        /// Set the vertex directions based on the orientation (no need
        /// to calculate since the algorithm to calculate the verticies
        /// always goes in the same order for a given orientation)
        /// </summary>
        private void SetVerticieDirections()
        {
            if (Orientation == HexOrientation.FlatTopped)
            {
                VerticeDirections = new List<HexDirection>
                    {
                        HexDirection.E,
                        HexDirection.NE,
                        HexDirection.NW,
                        HexDirection.W,
                        HexDirection.SW,
                        HexDirection.SE
                    };
            }
            else
            {
                VerticeDirections = new List<HexDirection>
                    {
                        HexDirection.NE,
                        HexDirection.N,
                        HexDirection.NW,
                        HexDirection.SW,
                        HexDirection.S,
                        HexDirection.SE
                    };
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

        /// <summary>
        /// Set the face directions based on the orientation (no need
        /// to calculate since the algorithm to calculate the verticies
        /// always goes in the same order for a given orientation)
        /// </summary>
        private void SetFaceDirections()
        {
            if (Orientation == HexOrientation.FlatTopped)
            {
                FaceDirections = new List<HexDirection>
                    {
                        HexDirection.NE,
                        HexDirection.N,
                        HexDirection.NW,
                        HexDirection.SW,
                        HexDirection.S,
                        HexDirection.SE
                    };
            }
            else
            {
                FaceDirections = new List<HexDirection>
                    {
                        HexDirection.NE,
                        HexDirection.NW,
                        HexDirection.W,
                        HexDirection.SW,
                        HexDirection.SE,
                        HexDirection.E
                    };
            }
        }

        #endregion
    }

    /// <summary>
    /// This extends HexMapItem with a generic Value property
    /// </summary>
    public class HexMapItem<T> : HexMapItem
    {
        public T Value { get; set; }

        public HexMapItem(HexOrientation orientation, double size, int x, int y, int z)
            : base(orientation, size, x, y, z)
        {
            
        }
    }
}
