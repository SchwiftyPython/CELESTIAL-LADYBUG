using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class TerrainStore : MonoBehaviour
    {
        private readonly Dictionary<BiomeType, List<TileType>> _floorTileTypesDictionary =
            new Dictionary<BiomeType, List<TileType>>
            {
                {BiomeType.Grassland, new List<TileType> {TileType.Grass, TileType.Mud}}
            };

        private Dictionary<BiomeType, Dictionary<TileType, int>> _tileTypeWeights =
            new Dictionary<BiomeType, Dictionary<TileType, int>>
            {
                {BiomeType.Grassland, new Dictionary<TileType, int>
                {
                    {TileType.Grass, 110},
                    {TileType.Mud, 20},
                    //{TileType.Tree, 20},
                }}
            };

        public Floor GetRandomFloorTile(BiomeType bType, Coord position)
        {
            var tileTypes = _floorTileTypesDictionary[bType];

            return new Floor(tileTypes[Random.Range(0, tileTypes.Count)], position);
        }

        public Floor GetFloorTile(TileType tType, Coord position)
        {
            return new Floor(tType, position);
        }

        public Dictionary<TileType, int> GetTileTypeWeights(BiomeType bType)
        {
            return _tileTypeWeights[bType];
        }
    }
}
