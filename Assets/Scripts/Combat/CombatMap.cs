using GoRogue;
using GoRogue.GameFramework;

namespace Assets.Scripts.Combat
{
    public class CombatMap : Map
    {
        public CombatMap(int width, int height) : base(width, height, 1, Distance.CHEBYSHEV, 4294967295, 4294967295, 0)
        {
        }
    }
}
