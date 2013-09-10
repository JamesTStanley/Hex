using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex
{
    /// <summary>
    /// An item in a hex-shaped hex map. This will need some refactoring to
    /// work with maps that either aren't hexagonal in shape, or have some
    /// alternate coordinate system where the center hex isn't 0,0,0 (cube) 
    /// or 0,0 (axial).
    /// </summary>
    public class HexMapItem<T>
    {
        // Cube coordinates
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        // Derived axial coordinates
        public int Q { get { return X; } }
        public int R { get { return Z; } }

        public int Ring { get; private set; }

        public T Value { get; set; } 

        public HexMapItem(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            Ring = (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z))/2;
        }

        public HexMapItem(int x, int y, int z, T value) : this(x, y, z)
        {
            Value = value;
        }
    }
}
