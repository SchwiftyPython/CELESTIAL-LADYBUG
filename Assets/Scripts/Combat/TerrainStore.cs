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
                {BiomeType.Forest, new List<TileType> {TileType.Grass, TileType.Mud}},
                {BiomeType.Desert, new List<TileType> {TileType.Sand}},
            };

        private readonly Dictionary<BiomeType, Dictionary<TileType, int>> _tileTypeWeights =
            new Dictionary<BiomeType, Dictionary<TileType, int>>
            {
                {BiomeType.Forest, new Dictionary<TileType, int>
                {
                    {TileType.Grass, 110},
                    {TileType.GrassDecorators, 10},
                    {TileType.Mud, 5},
                    {TileType.Tree, 10},
                }},
                {BiomeType.Desert, new Dictionary<TileType, int>
                {
                    {TileType.Sand, 110},
                    {TileType.SandDecorators, 5},
                    {TileType.Rock, 5},
                    {TileType.Tree, 2},
                }}
            };

        public static Floor GetFloorTile(BiomeType bType, TileType tType, Coord position, int mapWidth, int mapHeight)
        {
            return new Floor(bType, tType, position, mapWidth, mapHeight);
        }

        public Wall GetWallTile(BiomeType bType, TileType tType, Coord position, int mapWidth, int mapHeight)
        {
            return new Wall(bType, tType, position, mapWidth, mapHeight);
        }

        public Dictionary<TileType, int> GetTileTypeWeights(BiomeType bType)
        {
            return _tileTypeWeights[bType];
        }
    }
}
