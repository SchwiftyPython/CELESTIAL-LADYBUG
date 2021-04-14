using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Items.Components;
using Leguar.TotalJSON;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Items
{
    public class ItemStore : MonoBehaviour, ISubscriber
    {
        private Dictionary<string, ItemType> _itemTypes;

        public TextAsset ItemTypesFile;

        public static ItemStore Instance;

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
            DontDestroyOnLoad(gameObject);

            EventMediator.Instance.SubscribeToEvent(GlobalHelper.SpritesLoaded, this);
        }

        public ItemType GetItemTypeByName(string typeName)
        {
            if (typeName == null || !_itemTypes.ContainsKey(typeName.ToLower()))
            {
                return null;
            }

            return _itemTypes[typeName.ToLower()];
        }

        public Item GetRandomItem()
        {
            var itemTypeValues = _itemTypes.Values.ToArray();

            return itemTypeValues[Random.Range(0, itemTypeValues.Length)].NewItem();
        }

        public EquipableItem GetRandomEquipableItem()
        {
            var itemTypeValues = new List<ItemType>();

            foreach (var itemType in _itemTypes.Values)
            {
                if (itemType.IsEquipable())
                {
                    itemTypeValues.Add(itemType);
                }
            }

            return (EquipableItem) itemTypeValues[Random.Range(0, itemTypeValues.Count)].NewItem();
        }

        public EquipableItem GetRandomEquipableItem(EquipLocation location)
        {
            var itemTypeValues = new List<ItemType>();

            foreach (var itemType in _itemTypes.Values)
            {
                if (itemType.Slot == location)
                {
                    itemTypeValues.Add(itemType);
                }
            }

            return (EquipableItem)itemTypeValues[Random.Range(0, itemTypeValues.Count)].NewItem();
        }

        private void DeserializeItemTypes()
        {
            _itemTypes = new Dictionary<string, ItemType>();

            var settings = new DeserializeSettings {RequireAllFieldsArePopulated = false};

            var json = JSON.ParseString(ItemTypesFile.text);

            foreach (var key in json.Keys)
            {
                var itemTypeJson = json.GetJSON(key);

                var itemType = new ItemType();

                itemType.Name = key;

                itemType.Description = itemTypeJson.GetString("Description")?.Trim();

                itemType.Parent = itemTypeJson.GetString("Parent")?.Trim();

                itemType.Melee = itemTypeJson.GetJSON("Melee").Deserialize<Attack>(settings);

                itemType.Ranged = itemTypeJson.GetJSON("Ranged").Deserialize<Attack>(settings);

                itemType.Defense = itemTypeJson.GetJSON("Defense").Deserialize<Defense>(settings);

                itemType.Abilities = new List<string>(itemTypeJson.GetJArray("Abilities").AsStringArray());

                itemType.Range = itemTypeJson.GetInt("Range");

                var spriteKey = itemTypeJson.GetString("Sprite")?.Trim();

                itemType.Sprite = SpriteStore.Instance.GetItemSpriteByKey(spriteKey);

                var slotString = itemTypeJson.GetString("Slot")?.Trim();

                itemType.Stackable = itemTypeJson.GetBool("Stackable");

                itemType.Price = itemTypeJson.GetInt("Price");

                itemType.TwoHanded = itemTypeJson.GetBool("TwoHanded");

                if (string.IsNullOrEmpty(slotString))
                {
                    itemType.Slot = null;
                }
                else
                {
                    itemType.Slot = (EquipLocation)Enum.Parse(typeof(EquipLocation), slotString, true);
                }

                _itemTypes.Add(key.ToLower(), itemType);
            }
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.SpritesLoaded))
            {
                DeserializeItemTypes();
            }
        }
    }
}
