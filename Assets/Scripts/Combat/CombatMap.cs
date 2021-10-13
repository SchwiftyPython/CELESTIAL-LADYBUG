using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.GameFramework;

namespace Assets.Scripts.Combat
{
    [Serializable]
    public class CombatMap : Map
    {
        public CombatMap(int width, int height) : base(width, height, 1, Distance.CHEBYSHEV, 4294967295, 4294967295, 0)
        {
            Direction.YIncreasesUpward = true;
        }

        public bool OutOfBounds(Coord targetCoord)
        {
            var (x, y) = targetCoord;

            return x >= Width || x < 0 || y >= Height || y < 0;
        }

        public Tile GetTileAt(Coord position)
        {
            if (OutOfBounds(position))
            {
                return null;
            }

            return GetTerrain<Tile>(position);
        }

        public List<Tile> GetRetreatTiles()
        {
            var retreatTiles = new List<Tile>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var tile = GetTerrain<Tile>(new Coord(x, y));

                    if (tile.RetreatTile)
                    {
                        retreatTiles.Add(tile);
                    }
                }
            }

            return retreatTiles;
        }
    }
}
