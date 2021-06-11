using System.Collections.Generic;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Floor : Tile
    {
        private readonly Dictionary<TileType, int> _terrainCosts = new Dictionary<TileType, int>
        {
            {TileType.Grass, 2},
            {TileType.Mud, 3},
        };

        public int ApCost { get; private set; }

        public Floor(TileType tileType, Coord position) : base(position, true, true)
        {
            TileType = tileType;

            ApCost = _terrainCosts[tileType];

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            var floorSprites = spriteStore.GetFloorSprites(tileType);

            Texture = floorSprites[Random.Range(0, floorSprites.Length)];
        }
    }
}
