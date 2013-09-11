using System;

namespace Hex
{
    /// <summary>
    /// An item in a hex-shaped hex map. This base class encapsulates all of the
    /// fundamentals that aren't concerned with translating a hex map into a
    /// display (size, width, height, etc.).
    /// This will need some refactoring (namely the Ring property) to work with 
    /// maps that either aren't hexagonal in shape, or have some alternate 
    /// coordinate system where the center hex isn't 0,0,0 (cube) or 0,0 (axial).
    /// </summary>
    public abstract class HexMapItemBase<T>
    {
        #region Constructors

        protected HexMapItemBase(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            Ring = (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z)) / 2;
        }

        protected HexMapItemBase(int x, int y, int z, T value)
            : this(x, y, z)
        {
            Value = value;
        }

        #endregion

        #region Public Properties

        #region Cube coordinates

        /// <summary>
        /// X-axis coordinate of the hex in a cube coordinate space
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y-axis coordinate of the hex in a cube coordinate space
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Z-axis coordinate of the hex in a cube coordinate space
        /// </summary>
        public int Z { get; private set; }

        #endregion

        #region Derived axial coordinates

        /// <summary>
        /// Q-axis coordinate of the hex in an axial coordinate space
        /// </summary>
        public int Q { get { return X; } }

        /// <summary>
        /// R-axis coordinate of the hex in an axial coordinate space
        /// </summary>
        public int R { get { return Z; } }

        #endregion

        /// <summary>
        /// Ring number the hex belongs to. Center hex == 0, neighbors
        /// of the center hex == 1, etc.
        /// </summary>
        public int Ring { get; private set; }

        /// <summary>
        /// Generic typed value that represents what's contained in a
        /// particular hex map item
        /// </summary>
        public T Value { get; set; }

        #endregion
    }
}
