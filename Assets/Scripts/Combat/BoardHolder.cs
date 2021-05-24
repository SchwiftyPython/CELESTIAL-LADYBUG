using Assets.Scripts.AI;
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

                    var tileInstance = Instantiate(tile.PrefabTexture, new Vector2(currentColumn, currentRow), Quaternion.identity);

                    tile.SetSpriteInstance(tileInstance);

                    tileInstance.AddComponent<OnMouseOverTile>();

                    tileInstance.GetComponent<OnMouseOverTile>().Tile = tile;

                    tileInstance.transform.SetParent(transform);

                    tileInstance.GetComponent<TerrainSlotUi>().SetTile(tile);

                    var entity = map.GetEntity<Entity>(coord);

                    if (entity != null)
                    {
                        var entityInstance = Instantiate(entity.CombatSpritePrefab, new Vector2(currentColumn, currentRow), Quaternion.identity);

                        entityInstance.transform.SetParent(EntityHolder);

                        entity.SetSpriteInstance(entityInstance);

                        if (!entity.IsPlayer())
                        {
                            entityInstance.AddComponent<AiController>();
                            entityInstance.GetComponent<AiController>().SetSelf(entity);
                        }

                        tileInstance.GetComponent<TerrainSlotUi>().SetEntity(entity);
                    }
                }
            }
        }
    }
}
