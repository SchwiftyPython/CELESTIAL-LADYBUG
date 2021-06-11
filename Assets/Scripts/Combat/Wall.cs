using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Wall : Tile
    {
        public Wall(TileType tileType, GameObject texture, Coord position) : base(position, false, false)
        {
        }
    }
}
