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
                    {TileType.GrassDecorators, 10},
                    {TileType.Mud, 5},
                    {TileType.Tree, 5},
                }}
            };

        public static Floor GetFloorTile(TileType tType, Coord position, int mapWidth, int mapHeight)
        {
            return new Floor(tType, position, mapWidth, mapHeight);
        }

        public Wall GetWallTile(TileType tType, Coord position, int mapWidth, int mapHeight)
        {
            return new Wall(tType, position, mapWidth, mapHeight);
        }

        public Dictionary<TileType, int> GetTileTypeWeights(BiomeType bType)
        {
            return _tileTypeWeights[bType];
        }
    }
}
