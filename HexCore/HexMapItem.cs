using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex
{
    public class HexMapItem<T>
    {
        // Cube coordinates
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        // Derived axial coordinates
        public int Q { get { return X; } }
        public int R { get { return Z; } }

        public T Value { get; set; } 

        public HexMapItem(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public HexMapItem(int x, int y, int z, T value) : this(x, y, z)
        {
            Value = value;
        }
    }
}
