using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Wall : Tile
    {
        public Wall(GameObject texture, Coord position) : base(texture, position, false, false)
        {
        }
    }
}
