using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Wall : Tile
    {
        public Wall(TileType tileType, Coord position, int mapWidth, int mapHeight) : base(position, false, false, mapWidth, mapHeight)
        {
            TileType = tileType;

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            var wallSprites = spriteStore.GetWallSprites(tileType);

            Texture = wallSprites[Random.Range(0, wallSprites.Length)];
        }
    }
}
