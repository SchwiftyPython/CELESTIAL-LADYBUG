using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class BoardHolder : MonoBehaviour
    {
        public Transform EntityHolder;

        public static BoardHolder Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Build(CombatMap map)
        {
            for (var currentColumn = 0; currentColumn < map.Width; currentColumn++)
            {
                for (var currentRow = 0; currentRow < map.Height; currentRow++)
                {
                    var coord = new Coord(currentColumn, currentRow);

                    var tile = map.GetTerrain<Tile>(coord);

                    var instance = Instantiate(tile.PrefabTexture, new Vector2(currentColumn, currentRow), Quaternion.identity);

                    tile.SetSpriteInstance(instance);

                    instance.AddComponent<OnMouseOverTile>();

                    instance.GetComponent<OnMouseOverTile>().Tile = tile;

                    instance.transform.SetParent(transform);

                    var entity = map.GetEntity<Entity>(coord);

                    if (entity != null)
                    {
                        instance = Instantiate(entity.CombatSpritePrefab, new Vector2(currentColumn, currentRow), Quaternion.identity);

                        instance.transform.SetParent(EntityHolder);

                        entity.SetSpriteInstance(instance);
                    }
                }
            }
        }
    }
}
