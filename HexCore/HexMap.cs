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
        
        /// <summary>
        /// Initialize a new hex map
        /// </summary>
        /// <param name="orientation">Flat topped or pointy topped orientation</param>
        /// <param name="ringCount">The number of rings around the hex in the center</param>
        /// <param name="hexSize">The size (distance from center to vertex) of a hex</param>
        public HexMap(HexOrientation orientation, int ringCount, double hexSize = 1)
        {
            if (ringCount < 0)
                throw new ArgumentOutOfRangeException("ringCount", "The ringCount param must be >= 0");

            Rings = ringCount;

            Map = new List<HexMapItem<T>>();
            
            // Generate the map items. Loop over x, y, and z coordinates from -ringCount
            // to +ringCount, but skipping any cases where x + y + z != 0
            for (int x = -1 * ringCount; x <= ringCount; x++)
            {
                for (int y = -1 * ringCount; y <= ringCount; y++)
                    for (int z = -1 * ringCount; z <= ringCount; z++ )
                        if(x + y + z == 0)
                            Map.Add(new HexMapItem<T>(orientation, hexSize, x, y, z));
            }
        }

        public int Rings { get; private set; }
        public List<HexMapItem<T>> Map { get; private set; }

        /// <summary>
        /// Get an item from the hex map by its cube coordinates
        /// </summary>
        /// <returns>The map item at these coordinates, or null if there is no location at these coordinates</returns>
        public HexMapItem<T> Item(int x, int y, int z)
        {
            return Map.FirstOrDefault(i => i.X == x && i.Y == y && i.Z == z);
        }

        /// <summary>
        /// Get an item from the hex map by its axial coordinates
        /// </summary>
        /// <returns>The map item at these coordinates, or null if there is no location at these coordinates</returns>
        public HexMapItem<T> Item(int q, int r)
        {
            return Map.FirstOrDefault(i => i.Q == q && i.R == r);
        }

        /// <summary>
        /// Returns all of the map items in a ring from the center
        /// </summary>
        /// <param name="ringNumber">0 returns the center item, 1 the 6 neighboars of the center item, etc.</param>
        /// <returns>A list of hex map items, or null if the ring specified isn't in the map.</returns>
        public List<HexMapItem<T>> Ring(int ringNumber)
        {
            return Map.Where(i => i.Ring == ringNumber).ToList();
        }
    }
}