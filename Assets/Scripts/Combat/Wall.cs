using Assets.Scripts.Travel;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class Wall : Tile
    {
        public Wall()
        {
        }

        public Wall(BiomeType biomeType, TileType tileType, Coord position, int mapWidth, int mapHeight) : base(position, false, false, mapWidth, mapHeight)
        {
            TileType = tileType;

            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            var wallSprites = spriteStore.GetWallSprites(biomeType, tileType);

            Texture = wallSprites[Random.Range(0, wallSprites.Length)];
        }

        public new object CaptureState()
        {
            var dto = new TileDto
            {
                Texture = Texture,
                BType = BiomeType,
                IsFloor = false,
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
                false, false);

            TileType = dto.TType;

            RetreatTile = false;
        }
    }
}
