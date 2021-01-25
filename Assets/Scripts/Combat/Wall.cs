using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Wall : Tile
    {
        //todo get prefab texture from terrain store based on tile type
        public Wall(TileType tileType, GameObject texture, Coord position) : base(tileType, texture, position, false, false)
        {
        }
    }
}
