using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Floor : Tile
    {
        public Floor(GameObject texture, Coord position) : base(texture, position, true, true)
        {
        }
    }
}
