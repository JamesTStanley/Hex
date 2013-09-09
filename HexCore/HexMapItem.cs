using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex
{
    public class HexMapItem<T>
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
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
