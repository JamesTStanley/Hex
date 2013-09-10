using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HexCoreTests")]

namespace Hex
{
    // Future: Deal with triangle, rectangle, rhombus, and arbitrary
    // shapes for the map

    public class HexMap<T>
    {
        internal List<HexMapItemBase<T>> _map;
        public int Rings { get; private set; }
        public List<HexMapItemBase<T>> Map { get { return _map; } }

        /// <summary>
        /// Initialize a new hex map
        /// </summary>
        /// <param name="orientation">Flat topped or pointy topped orientation</param>
        /// <param name="sizeFactor">This is the number of rings around the hex in the center</param>
        public HexMap(HexOrientation orientation, int sizeFactor, double hexSize = 1)
        {
            if (sizeFactor < 0)
                throw new ArgumentOutOfRangeException("sizeFactor", "The sizeFactor param must be >= 1");
            
            Rings = sizeFactor + 1;

            _map = new List<HexMapItemBase<T>>();
            
            // Generate the map items. Loop over x, y, and z coordinates from -sizeFactor
            // to +sizeFactor, but skipping any cases where x + y + z != 0
            for (int x = -1 * sizeFactor; x <= sizeFactor; x++)
            {
                for (int y = -1 * sizeFactor; y <= sizeFactor; y++)
                    for (int z = -1 * sizeFactor; z <= sizeFactor; z++ )
                        if(x + y + z == 0)
                            _map.Add(new HexMapItem<T>(orientation, hexSize, x, y, z));
            }
        }

        /// <summary>
        /// Get an item from the hex map by its cube coordinates
        /// </summary>
        /// <returns>The map item at these coordinates, or null if there is no location at these coordinates</returns>
        public HexMapItemBase<T> Item(int x, int y, int z)
        {
            return _map.FirstOrDefault(i => i.X == x && i.Y == y && i.Z == z);
        }

        /// <summary>
        /// Returns all of the map items in a ring from the center
        /// </summary>
        /// <param name="ringNumber">0 returns the center item, 1 the 6 neighboard of the center item, etc.</param>
        /// <returns>A list of hex map items, or null if the ring specified isn't in the map.</returns>
        public List<HexMapItemBase<T>> Ring(int ringNumber)
        {
            return _map.Where(i => i.Ring == ringNumber).ToList();
        }
    }
}