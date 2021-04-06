using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class SpriteStore : MonoBehaviour
    {
        //todo need a struct to hold the sprite and the color scheme that goes with it

        #region PortraitSprites

        private Dictionary<string, Sprite> _skinDictionary;
        private Dictionary<string, Sprite> _earDictionary;
        private Dictionary<string, Sprite> _chestDictionary;
        private Dictionary<string, Sprite> _facialHairDictionary;
        private Dictionary<string, Sprite> _hairDictionary;
        private Dictionary<string, Sprite> _helmetDictionary;

        public Sprite[] SkinSprites;
        public Sprite[] EarSprites;
        public Sprite[] ChestSprites;
        public Sprite[] FacialFairSprites;
        public Sprite[] HairSprites;
        public Sprite[] HelmetSprites;

        #endregion PortraitSprites

        #region ItemSprites

        private List<Dictionary<string, Sprite>> _allItemSprites;

        private Dictionary<string, Sprite> _swordSpriteDictionary;
        private Dictionary<string, Sprite> _robeSpriteDictionary;
        private Dictionary<string, Sprite> _headbandSpriteDictionary;

        public Sprite[] SwordSprites;
        public Sprite[] RobeSprites;
        public Sprite[] HeadBandSprites;

        #endregion ItemSprites

        public static SpriteStore Instance;

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
            DontDestroyOnLoad(gameObject);
            
            PopulateSpriteDictionaries();

            EventMediator.Instance.Broadcast(GlobalHelper.SpritesLoaded, this);
        }

        public Sprite GetRandomSpriteForSlot(Portrait.Slot slot)
        {
            switch (slot)
            {
                case Portrait.Slot.Skin:
                    return SkinSprites[Random.Range(0, SkinSprites.Length)];
                case Portrait.Slot.Ears:
                    return EarSprites[Random.Range(0, EarSprites.Length)]; 
                case Portrait.Slot.Chest:
                    return ChestSprites[Random.Range(0, ChestSprites.Length)];
                case Portrait.Slot.FacialHair:
                    return FacialFairSprites[Random.Range(0, FacialFairSprites.Length)];
                case Portrait.Slot.Hair:
                    return HairSprites[Random.Range(0, HairSprites.Length)];
                case Portrait.Slot.Helmet:
                    return HelmetSprites[Random.Range(0, HelmetSprites.Length)];
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public string GetRandomSpriteKeyForSlot(Portrait.Slot slot)
        {
            switch (slot)
            {
                case Portrait.Slot.Skin:
                    return _skinDictionary.ElementAt(Random.Range(0, _skinDictionary.Count)).Key;
                case Portrait.Slot.Ears:
                    return _earDictionary.ElementAt(Random.Range(0, _earDictionary.Count)).Key;
                case Portrait.Slot.Chest:
                    return _chestDictionary.ElementAt(Random.Range(0, _chestDictionary.Count)).Key;
                case Portrait.Slot.FacialHair:
                    return _facialHairDictionary.ElementAt(Random.Range(0, _facialHairDictionary.Count)).Key;
                case Portrait.Slot.Hair:
                    return _hairDictionary.ElementAt(Random.Range(0, _hairDictionary.Count)).Key;
                case Portrait.Slot.Helmet:
                    return _helmetDictionary.ElementAt(Random.Range(0, _helmetDictionary.Count)).Key;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public Sprite GetPortraitSpriteForSlotByKey(Portrait.Slot slot, string key)
        {
            switch (slot)
            {
                case Portrait.Slot.Skin:
                    return _skinDictionary[key];
                case Portrait.Slot.Ears:
                    return _earDictionary[key];
                case Portrait.Slot.Chest:
                    return _chestDictionary[key];
                case Portrait.Slot.FacialHair:
                    return _facialHairDictionary[key];
                case Portrait.Slot.Hair:
                    return _hairDictionary[key];
                case Portrait.Slot.Helmet:
                    return _helmetDictionary[key];
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
            var index = Random.Range(0, _swordSpriteDictionary.Count);

            return _swordSpriteDictionary.ElementAt(index).Value;
        }

        private void PopulateSpriteDictionaries()
        {
            PopulatePortraitSprites();
            PopulateItemSprites();
        }

        private void PopulatePortraitSprites()
        {
            foreach(Portrait.Slot slot in Enum.GetValues(typeof(Portrait.Slot)))
            {
                switch (slot)
                {
                    case Portrait.Slot.Skin:
                        _skinDictionary = PopulateDictionaryFromArray(SkinSprites);
                        break;
                    case Portrait.Slot.Ears:
                        _earDictionary = PopulateDictionaryFromArray(EarSprites);
                        break;
                    case Portrait.Slot.Chest:
                        _chestDictionary = PopulateDictionaryFromArray(ChestSprites);
                        break;
                    case Portrait.Slot.FacialHair:
                        _facialHairDictionary = PopulateDictionaryFromArray(FacialFairSprites);
                        break;
                    case Portrait.Slot.Hair:
                        _hairDictionary = PopulateDictionaryFromArray(HairSprites);
                        break;
                    case Portrait.Slot.Helmet:
                        _helmetDictionary = PopulateDictionaryFromArray(HelmetSprites);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
                }
            }
        }

        private void PopulateItemSprites()
        {
            _swordSpriteDictionary = PopulateDictionaryFromArray(SwordSprites);

            _robeSpriteDictionary = PopulateDictionaryFromArray(RobeSprites);

            _headbandSpriteDictionary = PopulateDictionaryFromArray(HeadBandSprites);

            _allItemSprites = new List<Dictionary<string, Sprite>>();

            _allItemSprites.AddRange(new List<Dictionary<string, Sprite>>
                {
                    _swordSpriteDictionary,
                    _robeSpriteDictionary,
                    _headbandSpriteDictionary
                }
            );
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
