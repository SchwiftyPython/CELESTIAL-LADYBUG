using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Combat
{
    public class MapGenerator : MonoBehaviour
    {
        private const int MapWidth = 32;
        private const int MapHeight = 24;

        public static MapGenerator Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public CombatMap Generate(List<Entity> combatants)
        {
            var map = GenerateTerrain();

            PlaceEntities(map, combatants);

            return map;
        }

        //todo temp for prototype
        private CombatMap GenerateTerrain()
        {
            var terrainMap = new ArrayMap<bool>(MapWidth, MapHeight);
            QuickGenerators.GenerateRectangleMap(terrainMap);

            var map = new CombatMap(terrainMap.Width, terrainMap.Height);

            var biome = FindObjectOfType<TravelManager>().CurrentBiome;

            var tStore = FindObjectOfType<TerrainStore>();

            var tileWeights = tStore.GetTileTypeWeights(biome);

            foreach (var position in terrainMap.Positions())
            {
                var selection = tileWeights.First().Key;

                var totalWeight = tileWeights.Values.Sum();

                var roll = Random.Range(0, totalWeight);

                foreach (var tType in tileWeights.OrderByDescending(t => t.Value))
                {
                    var weightedValue = tType.Value;

                    if (roll >= weightedValue)
                    {
                        roll -= weightedValue;
                    }
                    else
                    {
                        selection = tType.Key;
                        break;
                    }
                }

                Tile tile;
                if (IsWallTile(selection))
                {
                    tile = tStore.GetWallTile(selection, position);
                }
                else
                {
                    tile = TerrainStore.GetFloorTile(selection, position);
                }

                map.SetTerrain(tile);
            }

            return map;
        }

        //todo should probably be in Tile class
        private static bool IsWallTile(TileType tType)
        {
            switch (tType)
            {
                case TileType.Grass:
                case TileType.GrassDecorators:
                case TileType.Mud:
                    return false;
                case TileType.Tree:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tType), tType, null);
            }
        }

        private void PlaceEntities(CombatMap map, List<Entity> combatants)
        {
            const int maxTries = 3;

            //todo can come up with different deployments based on encounter.
            //todo for now just place on opposite sides

            (int, int) playerEntityRangeX = (0, MapWidth / 2);
            (int, int) enemyEntityRangeX = (MapWidth / 2 + 1, MapWidth - 5);

            int playerIndexX = playerEntityRangeX.Item2 - 1;
            int playerIndexY = MapHeight / 2 + combatants.Count / 3;

            foreach (var combatant in combatants)
            {
                if (combatant.IsDerpus())
                {
                    continue;
                }

                if (combatant.IsPlayer())
                {
                    var placed = false;
                    var numTries = 0;
                    while (!placed)
                    {
                        combatant.Position = new Coord(playerIndexX, playerIndexY);

                        placed = map.AddEntity(combatant);

                        if (playerIndexY <= 0 || numTries > maxTries)
                        {
                            playerIndexY = MapHeight / 2 + combatants.Count / 3;
                            playerIndexX--;
                        }
                        else
                        {
                            playerIndexY--;
                        }

                        numTries++;
                    }
                }
                else
                {
                    var (xMin, xMax) = enemyEntityRangeX;

                    var placed = false;
                    var numTries = 0;
                    while (!placed && numTries < maxTries)
                    {
                        combatant.Position = new Coord(Random.Range(xMin, xMax),
                            Random.Range(5, map.Height - 5));

                        placed = map.AddEntity(combatant);

                        numTries++;
                    }
                }
            }

        }

    }
}
