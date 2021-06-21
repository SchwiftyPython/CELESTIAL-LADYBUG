using GoRogue;
using GoRogue.GameFramework;

namespace Assets.Scripts.Combat
{
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
    }
}
