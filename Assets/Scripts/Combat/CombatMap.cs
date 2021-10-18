using System.Collections.Generic;
using Assets.Scripts.Saving;
using GoRogue;
using GoRogue.GameFramework;

namespace Assets.Scripts.Combat
{
    public class CombatMap : Map, ISaveable
    {
        private struct CombatMapDto
        {
            public List<object> Terrain;
        }

        public CombatMap(int width, int height) : base(width, height, 1, Distance.CHEBYSHEV, 4294967295, 4294967295, 0)
        {
            Direction.YIncreasesUpward = true;
        }

        public bool OutOfBounds(Coord targetCoord)
        {
            var (x, y) = targetCoord;

            return x >= Width || x < 0 || y >= Height || y < 0;
        }

        public Tile GetTileAt(Coord position)
        {
            if (OutOfBounds(position))
            {
                return null;
            }

            return GetTerrain<Tile>(position);
        }

        public List<Tile> GetRetreatTiles()
        {
            var retreatTiles = new List<Tile>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var tile = GetTerrain<Tile>(new Coord(x, y));

                    if (tile.RetreatTile)
                    {
                        retreatTiles.Add(tile);
                    }
                }
            }

            return retreatTiles;
        }

        public void PlaceEntitiesFromSave()
        {
            var combatManager = UnityEngine.Object.FindObjectOfType<CombatManager>();

            var entities = combatManager.TurnOrder; 

            foreach (var entity in entities)
            {
                var placed = AddEntity(entity);
            }
        }

        public object CaptureState()
        {
            var dto = new CombatMapDto
            {
                Terrain = new List<object>()
            };

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var coord = new Coord(x, y); 

                    var floor = GetTerrain<Floor>(coord);

                    if (floor != null)
                    {
                        dto.Terrain.Add(floor.CaptureState());
                        continue;
                    }

                    var wall = GetTerrain<Wall>(coord);

                    if (wall != null)
                    {
                        dto.Terrain.Add(wall.CaptureState());
                    }
                }
            }

            return dto;
        }

        public void RestoreState(object state)
        {
            CombatMapDto combatMapDto = (CombatMapDto)state;

            foreach (var tile in combatMapDto.Terrain)
            {
                var tileDto = (Tile.TileDto)tile;

                Tile restoredTile;

                if (tileDto.IsFloor)
                {
                    restoredTile = new Floor();
                    ((Floor)restoredTile).RestoreState(tileDto);
                    ((Floor)restoredTile).SetApCost();
                }
                else
                {
                    restoredTile = new Wall();
                    ((Wall)restoredTile).RestoreState(tileDto);
                }

                restoredTile.Texture = tileDto.Texture;

                SetTerrain(restoredTile);
            }

            PlaceEntitiesFromSave();
        }
    }
}
