using Assets.Scripts.Travel;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Wall : Tile
    {
        public Wall(BiomeType biomeType, TileType tileType, Coord position, int mapWidth, int mapHeight) : base(position, false, false, mapWidth, mapHeight)
        {
            TileType = tileType;

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            var wallSprites = spriteStore.GetWallSprites(biomeType, tileType);

            Texture = wallSprites[Random.Range(0, wallSprites.Length)];
        }
    }
}
