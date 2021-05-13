using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Effects;
using Assets.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class SpriteStore : MonoBehaviour
    {
        //todo need a struct to hold the sprite and the color scheme that goes with it
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

        private Dictionary<string, Sprite> _abilitySpriteDictionary;
        private Dictionary<string, Sprite> _effectSpriteDictionary;

        public Sprite[] AbilitySprites;
        public Sprite[] EffectSprites;

        public void Setup()
        {
            PopulateSpriteDictionaries();

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
                case Portrait.Slot.Ears:
                    return PortraitEarSprites[Random.Range(0, PortraitEarSprites.Length)]; 
                case Portrait.Slot.Chest:
                    return PortraitChestSprites[Random.Range(0, PortraitChestSprites.Length)];
                case Portrait.Slot.FacialHair:
                    return PortraitFacialFairSprites[Random.Range(0, PortraitFacialFairSprites.Length)];
                case Portrait.Slot.Hair:
                    return PortraitHairSprites[Random.Range(0, PortraitHairSprites.Length)];
                case Portrait.Slot.Helmet:
                    return PortraitHelmetSprites[Random.Range(0, PortraitHelmetSprites.Length)];
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
                case Portrait.Slot.Ears:
                    return _pEarDictionary.ElementAt(Random.Range(0, _pEarDictionary.Count)).Key;
                case Portrait.Slot.Chest:
                    return _pChestDictionary.ElementAt(Random.Range(0, _pChestDictionary.Count)).Key;
                case Portrait.Slot.FacialHair:
                    return _pFacialHairDictionary.ElementAt(Random.Range(0, _pFacialHairDictionary.Count)).Key;
                case Portrait.Slot.Hair:
                    return _pHairDictionary.ElementAt(Random.Range(0, _pHairDictionary.Count)).Key;
                case Portrait.Slot.Helmet:
                    return _pHelmetDictionary.ElementAt(Random.Range(0, _pHelmetDictionary.Count)).Key;
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
                case Portrait.Slot.Ears:
                    return _pEarDictionary[key];
                case Portrait.Slot.Chest:
                    return _pChestDictionary[key];
                case Portrait.Slot.FacialHair:
                    return _pFacialHairDictionary[key];
                case Portrait.Slot.Hair:
                    return _pHairDictionary[key];
                case Portrait.Slot.Helmet:
                    return _pHelmetDictionary[key];
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
                Debug.LogError($"Ability sprite for {ability.GetType()} does not exist!");
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
                    case Portrait.Slot.Ears:
                        _pEarDictionary = PopulateDictionaryFromArray(PortraitEarSprites);
                        break;
                    case Portrait.Slot.Chest:
                        _pChestDictionary = PopulateDictionaryFromArray(PortraitChestSprites);
                        break;
                    case Portrait.Slot.FacialHair:
                        _pFacialHairDictionary = PopulateDictionaryFromArray(PortraitFacialFairSprites);
                        break;
                    case Portrait.Slot.Hair:
                        _pHairDictionary = PopulateDictionaryFromArray(PortraitHairSprites);
                        break;
                    case Portrait.Slot.Helmet:
                        _pHelmetDictionary = PopulateDictionaryFromArray(PortraitHelmetSprites);
                        break;
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
    }
}
