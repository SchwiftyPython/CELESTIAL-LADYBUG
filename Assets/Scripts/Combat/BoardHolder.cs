using System;
using System.Collections.Generic;
using Assets.Scripts.AI;
using Assets.Scripts.Audio;
using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class BoardHolder : MonoBehaviour
    {
        public GameObject TerrainSlotPrefab;

        public Transform EntityHolder;

        public static BoardHolder Instance;

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
        }

        public void Build(CombatMap map)
        {
            GlobalHelper.DestroyAllChildren(gameObject);
            GlobalHelper.DestroyAllChildren(EntityHolder.gameObject);

            for (var currentColumn = 0; currentColumn < map.Width; currentColumn++)
            {
                for (var currentRow = 0; currentRow < map.Height; currentRow++)
                {
                    var coord = new Coord(currentColumn, currentRow);

                    var tile = map.GetTerrain<Tile>(coord);

                    var tileInstance = Instantiate(TerrainSlotPrefab, new Vector2(currentColumn, currentRow), Quaternion.identity);

                    tileInstance.GetComponent<SpriteRenderer>().sprite = tile.Texture;

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

                            var spriteRenderer = entityInstance.GetComponent<SpriteRenderer>();

                            if (spriteRenderer == null)
                            {
                                spriteRenderer = entityInstance.GetComponentInChildren<SpriteRenderer>();
                            }

                            spriteRenderer.flipX = true;
                            
                            var position = entityInstance.transform.position;

                            position = new Vector3(position.x + 1,
                                position.y, position.z);

                            entityInstance.transform.position = position;

                            var sRenderer = entityInstance.GetComponent<Renderer>();

                            if (sRenderer == null)
                            {
                                sRenderer = entityInstance.GetComponentInChildren<Renderer>();
                            }

                            var palette = FindObjectOfType<Palette>();

                            var mat = sRenderer.material;

                            mat.SetColor("_OutlineColor", palette.BrightRed);
                            mat.SetFloat("_OutlineWidth", 0.0009f);
                            mat.SetFloat("_OutlineAlpha", 1.0f);
                        }

                        entityInstance.AddComponent<EntityAudio>();

                        var entityAudio = entityInstance.GetComponent<EntityAudio>();

                        entityAudio.AttackSound = entity.AttackSound;
                        entityAudio.HurtSound = entity.HurtSound;
                        entityAudio.DeathSound = entity.DieSound;

                        var spriteStore = FindObjectOfType<SpriteStore>();

                        var colorSwapper = entityInstance.GetComponentsInChildren<ColorSwapper>();

                        spriteStore.SetColorSwaps(colorSwapper, entity);

                        var animationHelper = entityInstance.GetComponent<CombatAnimationHelper>();

                        if (animationHelper != null)
                        {
                            animationHelper.Parent = entity;
                        }

                        tileInstance.GetComponent<TerrainSlotUi>().SetEntity(entity);
                    }
                }
            }
        }
    }
}
