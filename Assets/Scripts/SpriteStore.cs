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

        public Sprite GetSpriteForSlotByKey(Portrait.Slot slot, string key)
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

        private void PopulateSpriteDictionaries()
        {
            PopulatePortraitSprites();
        }

        private void PopulatePortraitSprites()
        {
            foreach(Portrait.Slot slot in Enum.GetValues(typeof(Portrait.Slot)))
            {
                switch (slot)
                {
                    case Portrait.Slot.Skin:
                        _skinDictionary = new Dictionary<string, Sprite>
                        {
                            { "base", null }
                        };

                        PopulateDictionaryFromArray(_skinDictionary, SkinSprites);
                        break;
                    case Portrait.Slot.Ears:
                        _earDictionary = new Dictionary<string, Sprite>
                        {
                            { "human", null}
                        };

                        PopulateDictionaryFromArray(_earDictionary, EarSprites);
                        break;
                    case Portrait.Slot.Chest:
                        _chestDictionary = new Dictionary<string, Sprite>
                        {
                            { "padded", null },
                            { "plate", null },
                            { "robe", null }
                        };

                        PopulateDictionaryFromArray(_chestDictionary, ChestSprites);
                        break;
                    case Portrait.Slot.FacialHair:
                        _facialHairDictionary = new Dictionary<string, Sprite>
                        {
                            { "big beard", null },
                            { "chinstrap", null },
                            { "full", null },
                            { "goatee", null },
                            { "handlebar", null }
                        };

                        PopulateDictionaryFromArray(_facialHairDictionary, FacialFairSprites);
                        break;
                    case Portrait.Slot.Hair:
                        _hairDictionary = new Dictionary<string, Sprite>
                        { 
                            { "balding", null },
                            { "long", null },
                            { "medium", null },
                            { "short", null }
                        };

                        PopulateDictionaryFromArray(_hairDictionary, HairSprites);
                        break;
                    case Portrait.Slot.Helmet:
                        _helmetDictionary = new Dictionary<string, Sprite>
                        {
                            { "padded", null },
                            { "plate", null },
                            { "skullcap", null }
                        };

                        PopulateDictionaryFromArray(_helmetDictionary, HelmetSprites);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
                }
            }
        }

        private static void PopulateDictionaryFromArray(Dictionary<string, Sprite> spriteDictionary, IReadOnlyList<Sprite> sprites)
        {
            var index = 0;
            foreach (var spriteName in spriteDictionary.Keys.ToArray())
            {
                spriteDictionary[spriteName] = sprites[index];
                index++;
            }
        }
    }
}
