using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Wall : Tile
    {
        public Wall(TileType tileType, Coord position) : base(position, false, false)
        {
            TileType = tileType;

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            var wallSprites = spriteStore.GetWallSprites(tileType);

            Texture = wallSprites[Random.Range(0, wallSprites.Length)];
        }
    }
}
