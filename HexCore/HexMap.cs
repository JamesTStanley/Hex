using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("HexCoreTests")]

namespace Hex
{
    /// <summary>
    /// The shape of the map to generate
    /// </summary>
    public enum MapShape
    {
        HexagonFlatTopped,
        HexagonPointyTopped
    };

    // Future: Deal with triangle, rectangle, rhombus, and arbitrary
    // shapes for the map

    public class HexMap<T>
    {
        internal List<HexMapItem<T>> _map;
        public int Rings { get; private set; }
        public List<HexMapItem<T>> Map { get { return _map; } }

        /// <summary>
        /// Initialize a new hex map
        /// </summary>
        /// <param name="shape">The shape of the map</param>
        /// <param name="sizeFactor">This is the number of rings around the hex in the center</param>
        public HexMap(MapShape shape, int sizeFactor)
        {
            if (sizeFactor < 0)
                throw new ArgumentOutOfRangeException("sizeFactor", "The sizeFactor param must be >= 1");
            
            // TODO: deal with other map shapes
            if (shape != MapShape.HexagonFlatTopped)
                throw new NotImplementedException("Only flat-topped maps are implemented at this time.");

            Rings = sizeFactor + 1;

            _map = new List<HexMapItem<T>>();
            
            // Generate the map items. Loop over x, y, and z coordinates from -sizeFactor
            // to +sizeFactor, but skipping any cases where x + y + z != 0
            for (int x = -1 * sizeFactor; x <= sizeFactor; x++)
            {
                for (int y = -1 * sizeFactor; y <= sizeFactor; y++)
                    for (int z = -1 * sizeFactor; z <= sizeFactor; z++ )
                        if(x + y + z == 0)
                            _map.Add(new HexMapItem<T>(x, y, z));
            }
        }

        /// <summary>
        /// Get an item from the hex map by its cube coordinates
        /// </summary>
        /// <returns>The map item at these coordinates, or null if there is no location at these coordinates</returns>
        public HexMapItem<T> Item(int x, int y, int z)
        {
            return _map.FirstOrDefault(i => i.X == x && i.Y == y && i.Z == z);
        }

        public List<HexMapItem<T>> Ring(int ringNumber)
        {
            return _map.Where(i => Math.Abs(i.X) + Math.Abs(i.Y) + Math.Abs(i.Z) == ringNumber * 2).ToList();
        }
    }
}