using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Floor : Tile
    {
        private readonly Dictionary<TileType, int> _terrainCosts = new Dictionary<TileType, int>
        {
            {TileType.Grass, 2},
            {TileType.GrassDecorators, 2},
            {TileType.Mud, 3},
            {TileType.Sand, 3},
            {TileType.SandDecorators, 3},
        };

        public int ApCost { get; private set; }

        public Floor(BiomeType biomeType, TileType tileType, Coord position, int mapWidth, int mapHeight) : base(position, true, true, mapWidth, mapHeight)
        {
            ApCost = _terrainCosts[tileType];

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            var floorSprites = spriteStore.GetFloorSprites(biomeType, tileType);

            Texture = floorSprites[Random.Range(0, floorSprites.Length)];

            if (tileType == TileType.GrassDecorators) //todo going to have to move this into setter once we add more stuff
            {
                TileType = TileType.Grass;
            }
            else if (tileType == TileType.SandDecorators)
            {
                TileType = TileType.Sand;
            }
            else
            {
                TileType = tileType;
            }
        }
    }
}
