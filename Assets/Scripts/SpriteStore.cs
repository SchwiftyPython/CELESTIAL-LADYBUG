using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using Assets.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class SpriteStore : MonoBehaviour
    {
        public struct ColorSwap
        {
            public Color RedSwap;
            public Color GreenSwap;
        }

        private Dictionary<string, ColorScheme> _colorSwapLookup;

        #region PortraitSprites

        private Dictionary<string, Sprite> _pSkinDictionary;
        private Dictionary<string, Sprite> _pEarDictionary;
        private Dictionary<string, Sprite> _pChestDictionary;
        private Dictionary<string, Sprite> _pFacialHairDictionary;
        private Dictionary<string, Sprite> _pHairDictionary;
        private Dictionary<string, Sprite> _pHelmetDictionary;

        public Sprite[] PortraitSkinSprites;
        public Sprite[] PortraitEarSprites;
        public Sprite[] PortraitChestSprites;
        public Sprite[] PortraitFacialFairSprites;
        public Sprite[] PortraitHairSprites;
        public Sprite[] PortraitHelmetSprites;

        #endregion PortraitSprites

        #region ItemSprites

        private List<Dictionary<string, Sprite>> _allItemSprites;

        private Dictionary<string, Sprite> _weaponSpriteDictionary;
        private Dictionary<string, Sprite> _bodySpriteDictionary;
        private Dictionary<string, Sprite> _headwearSpriteDictionary;
        private Dictionary<string, Sprite> _bootSpriteDictionary;
        private Dictionary<string, Sprite> _gloveSpriteDictionary;
        private Dictionary<string, Sprite> _shieldSpriteDictionary;
        private Dictionary<string, Sprite> _ringSpriteDictionary;

        public Sprite[] WeaponSprites;
        public Sprite[] BodySprites;
        public Sprite[] HeadwearSprites;
        public Sprite[] BootSprites;
        public Sprite[] GloveSprites;
        public Sprite[] ShieldSprites;
        public Sprite[] RingSprites;

        public List<string> CrossbowSpriteNames { get; private set; }
        public List<string> DaggerSpriteNames { get; private set; }
        public List<string> ShieldSpriteNames { get; private set; }
        public List<string> SpearSpriteNames { get; private set; }
        public List<string> SwordSpriteNames { get; private set; }

        #endregion ItemSprites

        #region TerrainSprites

        public Sprite[] GrassSprites;
        public Sprite[] GrassDecoratorSprites;
        public Sprite[] GrassyMudSprites;
        public Sprite[] TemperateTreeSprites;

        private Dictionary<TileType, Sprite[]> _combatTerrainSprites;

        #endregion TerrainSprites

        private Dictionary<string, Sprite> _abilitySpriteDictionary;
        private Dictionary<string, Sprite> _effectSpriteDictionary;

        public Sprite[] AbilitySprites;
        public Sprite[] EffectSprites;

        public void Setup()
        {
            PopulateSpriteDictionaries();
            PopulateColorSwaps();
            PopulateCombatTerrain();

            CrossbowSpriteNames = new List<string>();
            DaggerSpriteNames = new List<string>();
            ShieldSpriteNames = new List<string>();
            SpearSpriteNames = new List<string>();
            SwordSpriteNames = new List<string>();

            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SpritesLoaded, this);
        }

        public Sprite GetRandomSpriteForSlot(Portrait.Slot slot)
        {
            switch (slot)
            {
                case Portrait.Slot.Skin:
                    return PortraitSkinSprites[Random.Range(0, PortraitSkinSprites.Length)];
                // case Portrait.Slot.Ears:
                //     return PortraitEarSprites[Random.Range(0, PortraitEarSprites.Length)]; 
                case Portrait.Slot.Chest:
                    return PortraitChestSprites[Random.Range(0, PortraitChestSprites.Length)];
                case Portrait.Slot.FacialHair:
                    return PortraitFacialFairSprites[Random.Range(0, PortraitFacialFairSprites.Length)];
                case Portrait.Slot.Hair:
                    return PortraitHairSprites[Random.Range(0, PortraitHairSprites.Length)];
                // case Portrait.Slot.Helmet:
                //     return PortraitHelmetSprites[Random.Range(0, PortraitHelmetSprites.Length)];
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public string GetRandomSpriteKeyForSlot(Portrait.Slot slot)
        {
            switch (slot)
            {
                case Portrait.Slot.Skin:
                    return _pSkinDictionary.ElementAt(Random.Range(0, _pSkinDictionary.Count)).Key;
                // case Portrait.Slot.Ears:
                //     return _pEarDictionary.ElementAt(Random.Range(0, _pEarDictionary.Count)).Key;
                case Portrait.Slot.Chest:
                    return _pChestDictionary.ElementAt(Random.Range(0, _pChestDictionary.Count)).Key;
                case Portrait.Slot.FacialHair:
                    return _pFacialHairDictionary.ElementAt(Random.Range(0, _pFacialHairDictionary.Count)).Key;
                case Portrait.Slot.Hair:
                    return _pHairDictionary.ElementAt(Random.Range(0, _pHairDictionary.Count)).Key;
                // case Portrait.Slot.Helmet:
                //     return _pHelmetDictionary.ElementAt(Random.Range(0, _pHelmetDictionary.Count)).Key;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public Sprite GetPortraitSpriteForSlotByKey(Portrait.Slot slot, string key)
        {
            switch (slot)
            {
                case Portrait.Slot.Skin:
                    return _pSkinDictionary[key];
                // case Portrait.Slot.Ears:
                //     return _pEarDictionary[key];
                case Portrait.Slot.Chest:
                    return _pChestDictionary[key];
                case Portrait.Slot.FacialHair:
                    return _pFacialHairDictionary[key];
                case Portrait.Slot.Hair:
                    return _pHairDictionary[key];
                // case Portrait.Slot.Helmet:
                //     return _pHelmetDictionary[key];
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public Sprite GetItemSpriteByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            foreach (var spriteDict in _allItemSprites)
            {
                if(spriteDict.ContainsKey(key))
                {
                    return spriteDict[key];
                }
            }

            Debug.LogError($"Sprite key {key} not found!");

            return null;
        }

        public Sprite GetRandomSwordSprite()
        {
            var index = Random.Range(0, _weaponSpriteDictionary.Count);

            return _weaponSpriteDictionary.ElementAt(index).Value;
        }

        public Sprite GetAbilitySprite(Ability ability)
        {
            if (_abilitySpriteDictionary == null || !_abilitySpriteDictionary.ContainsKey(ability.Name.ToLower()))
            {
                //Debug.LogError($"Ability sprite for {ability.GetType()} does not exist!");
                return null;
            }

            return _abilitySpriteDictionary[ability.Name.ToLower()];
        }

        public Sprite GetEffectSprite(Effect effect)
        {
            if (_effectSpriteDictionary == null || !_effectSpriteDictionary.ContainsKey(effect.Name.ToLower()))
            {
                Debug.LogError($"Effect sprite for {effect.GetType()} does not exist!");
                return null;
            }

            return _effectSpriteDictionary[effect.Name.ToLower()];
        }

        public Sprite[] GetFloorSprites(TileType tType)
        {
            return _combatTerrainSprites[tType];
        }

        public Sprite[] GetWallSprites(TileType tType)
        {
            return _combatTerrainSprites[tType];
        }

        public void SetColorSwaps(IEnumerable<ColorSwapper> colorSwapper, Entity entity)
        {
            foreach (var swapper in colorSwapper)
            {
                switch (swapper.swapSlot)
                {
                    case ColorSwapper.ColorSwapSlot.Helmet:
                        var helmet = entity.GetEquippedItemInSlot(EquipLocation.Helmet);

                        ColorScheme helmetColorSwap;
                        if (helmet == null || ReferenceEquals(helmet.GetIcon(), null))
                        {
                            helmetColorSwap = GetColorSwap(entity.Portrait[Portrait.Slot.Hair]);
                        }
                        else
                        {
                            helmetColorSwap = GetColorSwap(helmet.GetIcon().name);
                        }

                        swapper.SwapColorsOnSprite(helmetColorSwap);
                        break;
                    case ColorSwapper.ColorSwapSlot.Head:
                        var skinSprite = entity.Portrait[Portrait.Slot.Skin];

                        var skinColorSwap = GetColorSwap(skinSprite);

                        swapper.SwapColorsOnSprite(skinColorSwap);
                        break;
                    case ColorSwapper.ColorSwapSlot.Body:
                        var body = entity.GetEquippedItemInSlot(EquipLocation.Body);

                        ColorScheme bodyColorSwap;
                        if (body == null || ReferenceEquals(body.GetIcon(), null))
                        {
                            bodyColorSwap = GetColorSwap(entity.Portrait[Portrait.Slot.Skin]);
                        }
                        else
                        {
                            bodyColorSwap = GetColorSwap(body.GetIcon().name);
                        }

                        swapper.SwapColorsOnSprite(bodyColorSwap);
                        break;
                    case ColorSwapper.ColorSwapSlot.Hands:
                        var gloves = entity.GetEquippedItemInSlot(EquipLocation.Gloves);

                        ColorScheme handsColorSwap;
                        if (gloves == null || ReferenceEquals(gloves.GetIcon(), null))
                        {
                            handsColorSwap = GetColorSwap(entity.Portrait[Portrait.Slot.Skin]);
                        }
                        else
                        {
                            handsColorSwap = GetColorSwap(gloves.GetIcon().name);
                        }

                        swapper.SwapColorsOnSprite(handsColorSwap);
                        break;
                    case ColorSwapper.ColorSwapSlot.Feet:
                        var boots = entity.GetEquippedItemInSlot(EquipLocation.Boots);

                        ColorScheme bootColorSwap;
                        if (boots == null || ReferenceEquals(boots.GetIcon(), null))
                        {
                            bootColorSwap = GetColorSwap(entity.Portrait[Portrait.Slot.Skin]);
                        }
                        else
                        {
                            bootColorSwap = GetColorSwap(boots.GetIcon().name);
                        }

                        swapper.SwapColorsOnSprite(bootColorSwap);
                        break;
                    case ColorSwapper.ColorSwapSlot.Weapon:
                        var weapon = entity.GetEquippedWeapon();

                        if (weapon == null)
                        {
                            continue;
                        }

                        var weaponColorSwap = GetColorSwap(weapon.GetIcon().name);

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
        }

        public ColorScheme GetColorSwap(string key)
        {
            if (_colorSwapLookup == null || !_colorSwapLookup.ContainsKey(key))
            {
                return null;
            }

            return _colorSwapLookup[key];
        }

        private void PopulateSpriteDictionaries()
        {
            PopulatePortraitSprites();
            PopulateItemSprites();
            PopulateAbilitySprites();
            PopulateEffectSprites();
        }

        private void PopulatePortraitSprites()
        {
            foreach(Portrait.Slot slot in Enum.GetValues(typeof(Portrait.Slot)))
            {
                switch (slot)
                {
                    case Portrait.Slot.Skin:
                        _pSkinDictionary = PopulateDictionaryFromArray(PortraitSkinSprites);
                        break;
                    // case Portrait.Slot.Ears:
                    //     _pEarDictionary = PopulateDictionaryFromArray(PortraitEarSprites);
                    //     break;
                    case Portrait.Slot.Chest:
                        _pChestDictionary = PopulateDictionaryFromArray(PortraitChestSprites);
                        break;
                    case Portrait.Slot.FacialHair:
                        _pFacialHairDictionary = PopulateDictionaryFromArray(PortraitFacialFairSprites);
                        break;
                    case Portrait.Slot.Hair:
                        _pHairDictionary = PopulateDictionaryFromArray(PortraitHairSprites);
                        break;
                    // case Portrait.Slot.Helmet:
                    //     _pHelmetDictionary = PopulateDictionaryFromArray(PortraitHelmetSprites);
                    //     break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
                }
            }
        }

        private void PopulateItemSprites()
        {
            _weaponSpriteDictionary = PopulateDictionaryFromArray(WeaponSprites);

            _bodySpriteDictionary = PopulateDictionaryFromArray(BodySprites);

            _headwearSpriteDictionary = PopulateDictionaryFromArray(HeadwearSprites);

            _bootSpriteDictionary = PopulateDictionaryFromArray(BootSprites);

            _gloveSpriteDictionary = PopulateDictionaryFromArray(GloveSprites);

            _shieldSpriteDictionary = PopulateDictionaryFromArray(ShieldSprites);

            _ringSpriteDictionary = PopulateDictionaryFromArray(RingSprites);

            _allItemSprites = new List<Dictionary<string, Sprite>>();

            _allItemSprites.AddRange(new List<Dictionary<string, Sprite>>
                {
                    _weaponSpriteDictionary,
                    _bodySpriteDictionary,
                    _headwearSpriteDictionary,
                    _bootSpriteDictionary,
                    _gloveSpriteDictionary,
                    _shieldSpriteDictionary,
                    _ringSpriteDictionary
                }
            );
        }

        private void PopulateAbilitySprites()
        {
            _abilitySpriteDictionary = PopulateDictionaryFromArray(AbilitySprites);
        }

        private void PopulateEffectSprites()
        {
            _effectSpriteDictionary = PopulateDictionaryFromArray(EffectSprites);
        }

        private static Dictionary<string, Sprite> PopulateDictionaryFromArray(IEnumerable<Sprite> sprites)
        {
            var spriteDictionary = new Dictionary<string, Sprite>();

            foreach (var sprite in sprites)
            {
                spriteDictionary.Add(sprite.name, sprite);
            }

            return spriteDictionary;
        }

        private void PopulateCombatTerrain()
        {
            _combatTerrainSprites = new Dictionary<TileType, Sprite[]>
            {
                {TileType.Grass, GrassSprites},
                {TileType.GrassDecorators, GrassDecoratorSprites},
                {TileType.Mud, GrassyMudSprites},
                {TileType.Tree, TemperateTreeSprites}
            };
        }

        private void PopulateColorSwaps()
        {
            var palette = FindObjectOfType<Palette>();

            _colorSwapLookup = new Dictionary<string, ColorScheme>
            {
                {"base", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"base_2", new ColorScheme(palette.DarkModerateRed, palette.VeryDarkDesatPink, Color.white)},
                {"base_3", new ColorScheme(palette.VerySoftOrange, palette.SoftOrange, Color.white)},
                {"base_4", new ColorScheme(palette.BurntOrange, palette.DarkModerateRed, Color.white)},
                {"bear_head", new ColorScheme(palette.SoftOrange, palette.BurntOrange, Color.white)},
                {"bycocket", new ColorScheme(palette.BurntOrange, palette.DarkModerateRed, Color.white)},
                {"close_helm", new ColorScheme(palette.GrayBlue, palette.DarkDesatBlue, Color.white)},
                {"demon_helm", new ColorScheme(palette.ModeratePink, palette.SoftRed, Color.white)},
                {"insect_helmet", new ColorScheme(palette.BrightCyan, palette.PureBlue, Color.white)},
                {"leather_headband", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"leather_helmet", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"stone_mask", new ColorScheme(palette.LightGrayBlue, palette.GrayBlue, Color.white)},
                {"unicorn_helm", new ColorScheme(palette.LightGrayBlue, palette.GrayBlue, Color.white)},
                {"hair_balding", new ColorScheme(palette.VeryDarkDesatPink, palette.VeryDarkDesatPink, Color.white)},
                {"hair_long", new ColorScheme(palette.VeryDarkDesatMagenta, palette.DesatBlue, Color.white)},
                {"hair_long_2", new ColorScheme(palette.DarkModerateRed, palette.VeryDarkDesatPink, Color.white)},
                {"hair_short", new ColorScheme(palette.DarkModerateRed, palette.VeryDarkDesatPink, Color.white)},
                {"hair_short_2", new ColorScheme(palette.StrongRed, palette.DarkModerateRed, Color.white)},
                {"hair_short_3", new ColorScheme(palette.BrightOrange, palette.ModerateOrange, Color.white)},
                {"colorful_gloves", new ColorScheme(palette.VeryDarkDesatMagenta, palette.DesatBlue, Color.white)},
                {"fancy_bracer", new ColorScheme(palette.BrightRed, palette.DarkRed, Color.white)},
                {"fancy_gloves", new ColorScheme(palette.PureBlue, palette.DarkBlue, Color.white)},
                {"fancy_gloves_2", new ColorScheme(palette.DarkDesatBlue, palette.VeryDarkDesatBlue, Color.white)},
                {"fancy_gloves_3", new ColorScheme(palette.VeryDarkDesatMagenta, palette.VeryDarkDesatBlue, Color.white)},
                {"leather_gloves", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"leather_gloves_2", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"leather_gloves_3", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"metal_bracer", new ColorScheme(palette.DarkDesatBlue, palette.VeryDarkDesatBlue, Color.white)},
                {"plate_gauntlets_1", new ColorScheme(palette.DarkDesatBlue, palette.VeryDarkDesatBlue, Color.white)},
                {"lance", new ColorScheme(palette.GrayBlue, palette.DarkDesatBlue, Color.white)},
                {"naginata", new ColorScheme(palette.GrayBlue, palette.GrayBlue, Color.white)},
                {"spear", new ColorScheme(palette.GrayBlue, palette.DarkDesatBlue, Color.white)},
                {"demon_feet", new ColorScheme(palette.ModeratePink, palette.SoftRed, Color.white)},
                {"frost_boots", new ColorScheme(palette.PureBlue, palette.DarkBlue, Color.white)},
                {"grass_boots", new ColorScheme(palette.DarkLimeGreen, palette.VeryDarkLimeGreen, Color.white)},
                {"insect_boots", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"leather_boots", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"leather_boots_2", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"leather_boots_3", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"sexy_boots", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"speed_boots", new ColorScheme(palette.DarkRed, palette.VeryDarkDesatPink, Color.white)},
                {"robe_2", new ColorScheme(palette.BrightOrange, palette.DarkDesatBlue, Color.white)},
                {"robe_1", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"chainmail", new ColorScheme(palette.GrayBlue, palette.DarkDesatBlue, Color.white)},
                {"demon", new ColorScheme(palette.ModeratePink, palette.SoftRed, Color.white)},
                {"leather", new ColorScheme(palette.Orange, palette.DesatOrange, Color.white)},
                {"plate_armor", new ColorScheme(palette.GrayBlue, palette.DarkDesatBlue, Color.white)},
                {"plate_armor_2", new ColorScheme(palette.LightGrayBlue, palette.GrayBlue, Color.white)},
                {"plate_armor_3", new ColorScheme(palette.LightGrayBlue, palette.GrayBlue, Color.white)},
                {"plate_armor_4", new ColorScheme(palette.GrayBlue, palette.DarkDesatBlue, Color.white)},
            };
        }
    }
}
