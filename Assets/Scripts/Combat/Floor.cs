using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Floor : Tile
    {
        public int ApCost { get; private set; }

        //todo can get prefab texture from terrain store based on tiletype
        public Floor(TileType tileType, GameObject texture, Coord position) : base(tileType, texture, position, true, true)
        {
            //todo for testing prototype -- have terrain store return this info from a <TileType, int> dictionary
            ApCost = 2;
        }
    }
}
