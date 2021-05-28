using System;
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

                        //todo throw this section into a method -- prolly should be in sprite store

                        var spriteStore = FindObjectOfType<SpriteStore>();

                        var colorSwapper = entityInstance.GetComponentsInChildren<ColorSwapper>();

                        foreach (var swapper in colorSwapper)
                        {
                            switch (swapper.swapSlot)
                            {
                                case ColorSwapper.ColorSwapSlot.Helmet:
                                    var helmet = entity.GetEquippedItemInSlot(EquipLocation.Helmet);

                                    ColorScheme helmetColorSwap;
                                    if (helmet == null || ReferenceEquals(helmet.GetIcon(), null))
                                    {
                                        helmetColorSwap = spriteStore.GetColorSwap(entity.Portrait[Portrait.Slot.Hair]);
                                    }
                                    else
                                    {
                                        helmetColorSwap = spriteStore.GetColorSwap(helmet.GetIcon().name);
                                    }

                                    swapper.SwapColorsOnSprite(helmetColorSwap);
                                    break;
                                case ColorSwapper.ColorSwapSlot.Head:
                                    var skinSprite = entity.Portrait[Portrait.Slot.Skin];

                                    var skinColorSwap = spriteStore.GetColorSwap(skinSprite);

                                    swapper.SwapColorsOnSprite(skinColorSwap);
                                    break;
                                case ColorSwapper.ColorSwapSlot.Body:
                                    var body = entity.GetEquippedItemInSlot(EquipLocation.Body);

                                    ColorScheme bodyColorSwap;
                                    if (body == null || ReferenceEquals(body.GetIcon(), null))
                                    {
                                        bodyColorSwap = spriteStore.GetColorSwap(entity.Portrait[Portrait.Slot.Skin]);
                                    }
                                    else
                                    {
                                        bodyColorSwap = spriteStore.GetColorSwap(body.GetIcon().name);
                                    }

                                    swapper.SwapColorsOnSprite(bodyColorSwap);
                                    break;
                                case ColorSwapper.ColorSwapSlot.Hands:
                                    var gloves = entity.GetEquippedItemInSlot(EquipLocation.Gloves);

                                    ColorScheme handsColorSwap;
                                    if (gloves == null || ReferenceEquals(gloves.GetIcon(), null))
                                    {
                                        handsColorSwap = spriteStore.GetColorSwap(entity.Portrait[Portrait.Slot.Skin]);
                                    }
                                    else
                                    {
                                        handsColorSwap = spriteStore.GetColorSwap(gloves.GetIcon().name);
                                    }

                                    swapper.SwapColorsOnSprite(handsColorSwap);
                                    break;
                                case ColorSwapper.ColorSwapSlot.Feet:
                                    var boots = entity.GetEquippedItemInSlot(EquipLocation.Boots);

                                    ColorScheme bootColorSwap;
                                    if (boots == null || ReferenceEquals(boots.GetIcon(), null))
                                    {
                                        bootColorSwap = spriteStore.GetColorSwap(entity.Portrait[Portrait.Slot.Skin]);
                                    }
                                    else
                                    {
                                        bootColorSwap = spriteStore.GetColorSwap(boots.GetIcon().name);
                                    }

                                    swapper.SwapColorsOnSprite(bootColorSwap);
                                    break;
                                case ColorSwapper.ColorSwapSlot.Weapon:
                                    var weapon = entity.GetEquippedWeapon();

                                    if (weapon == null)
                                    {
                                        continue;
                                    }

                                    var weaponColorSwap = spriteStore.GetColorSwap(weapon.GetIcon().name);

                                    if (weaponColorSwap == null)
                                    {
                                        continue;
                                    }

                                    swapper.SwapColorsOnSprite(weaponColorSwap);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        tileInstance.GetComponent<TerrainSlotUi>().SetEntity(entity);
                    }
                }
            }
        }
    }
}
