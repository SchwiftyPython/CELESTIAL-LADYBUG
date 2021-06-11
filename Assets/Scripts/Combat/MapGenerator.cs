using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using UnityEngine;

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
            //todo change map generator if weighted stuff doesn't look so nice or is a mess to code

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

                //todo check if wall or floor

                var floor = tStore.GetFloorTile(selection, position);
                
                map.SetTerrain(floor);
            }

            return map;
        }

        private void PlaceEntities(CombatMap map, List<Entity> combatants)
        {
            const int maxTries = 3;

            //todo can come up with different deployments based on encounter.
            //todo for now just place on opposite sides

            (int, int) playerEntityRangeX = (0, MapWidth / 2);
            (int, int) enemyEntityRangeX = (MapWidth / 2 + 1, MapWidth);

            foreach (var combatant in combatants)
            {
                (int, int) entityRangeX;

                if (combatant.IsPlayer())
                {
                    entityRangeX = playerEntityRangeX;
                }
                else
                {
                    entityRangeX = enemyEntityRangeX;
                }

                var placed = false;
                var numTries = 0;
                while (!placed && numTries < maxTries)
                {
                    combatant.Position = new Coord(Random.Range(entityRangeX.Item1, entityRangeX.Item2),
                        Random.Range(1, map.Height));

                    placed = map.AddEntity(combatant);

                    numTries++;
                }

                // if (placed)
                // {
                //     Debug.Log($"{combatant.Name} placed at: {combatant.Position}");
                // }
                // else
                // {
                //     Debug.Log($"{combatant.Name} failed to place. Last try at: {combatant.Position}");
                // }
            }

        }

    }
}
