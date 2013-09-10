using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex
{
    public class HexMapItem<T> : HexMapItemBase<T>
    {
        public HexOrientation Orientation { get; private set; }
        public double Size { get; private set; }
        public Tuple<double, double> CenterPoint { get; private set; }
        public List<Tuple<double, double>> Vertices { get; private set; }
        public List<Tuple<Tuple<double, double>, Tuple<double, double>>> Faces { get; private set; } 

        public HexMapItem(HexOrientation orientation, double size, int x, int y, int z) : base(x, y, z)
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
    }
}
