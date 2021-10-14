using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

        public Floor()
        {
        }

        public Floor(BiomeType biomeType, TileType tileType, Coord position, int mapWidth, int mapHeight) : base(position, true, true, mapWidth, mapHeight)
        {
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

            SetApCost();
        }

        public void SetApCost()
        {
            ApCost = _terrainCosts[TileType];
        }

        public new object CaptureState()
        {
            var dto = new TileDto
            {
                Texture = Texture,
                BType = BiomeType,
                IsFloor = true,
                Position = new Vector2Int(Position.X, Position.Y),
                TType = TileType
            };

            return dto;
        }

        public new void RestoreState(object state)
        {
            var dto = (TileDto)state;

            Texture = dto.Texture;
            BiomeType = dto.BType;

            var position = new Coord(dto.Position.x, dto.Position.y);

            _backingField = new GoRogue.GameFramework.GameObject(position, 0, this, true,
                true, true);

            TileType = dto.TType;

            RetreatTile = IsEdge(MapGenerator.MapWidth, MapGenerator.MapHeight);
        }
    }
}
