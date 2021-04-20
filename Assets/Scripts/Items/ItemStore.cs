using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public TextAsset[] ItemTypesFiles;

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
            //todo store the sprite names associated with the item types in a dictionary for easy access

            _itemTypes = new Dictionary<string, ItemType>();

            var settings = new DeserializeSettings {RequireAllFieldsArePopulated = false};

            foreach (var itemTypesFile in ItemTypesFiles)
            {
                var json = JSON.ParseString(itemTypesFile.text);

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

                    itemType.Group = GetItemGroupFromFilename(itemTypesFile.name);

                    AddSpriteNameForGroup(itemType.Group, spriteKey);

                    if (string.IsNullOrEmpty(slotString))
                    {
                        itemType.Slot = null;
                    }
                    else
                    {
                        itemType.Slot = (EquipLocation) Enum.Parse(typeof(EquipLocation), slotString, true);
                    }

                    _itemTypes.Add(key.ToLower(), itemType);
                }
            }
        }

        private static ItemGroup GetItemGroupFromFilename(string filename)
        {
            switch (filename.ToLower())
            {
                case "robes":
                    return ItemGroup.Robe;
                case "armor":
                    return ItemGroup.Armor;
                case "crossbows":
                    return ItemGroup.Crossbow;
                case "daggers":
                    return ItemGroup.Dagger;
                case "feet":
                    return ItemGroup.Feet;
                case "gloves":
                    return ItemGroup.Glove;
                case "helmets":
                    return ItemGroup.Helmet;
                case "rings":
                    return ItemGroup.Ring;
                case "shields":
                    return ItemGroup.Shield;
                case "one_handed_spears":
                    return ItemGroup.OneHandedSpear;
                case "two_handed_spears":
                    return ItemGroup.TwoHandedSpear;
                case "one_handed_swords":
                    return ItemGroup.OneHandedSword;
                case "two_handed_swords":
                    return ItemGroup.TwoHandedSword;
                default:
                    Debug.LogError($"{filename} has no valid ItemGroup!");
                    throw new InvalidEnumArgumentException($"{filename} has no valid ItemGroup!");
            }
        }

        private void AddSpriteNameForGroup(ItemGroup group, string spriteName)
        {
            switch(group)
            {
                case ItemGroup.Robe:
                    //todo
                    break;
                case ItemGroup.Armor:
                    //todo
                    break;
                case ItemGroup.Feet:
                    //todo
                    break;
                case ItemGroup.Glove:
                    //todo
                    break;
                case ItemGroup.Helmet:
                    //todo
                    break;
                case ItemGroup.Ring:
                    //todo
                    break;
                case ItemGroup.Shield:
                    SpriteStore.Instance.ShieldSpriteNames.Add(spriteName);
                    break;
                case ItemGroup.Axe:
                    //todo
                    break;
                case ItemGroup.Crossbow:
                    SpriteStore.Instance.CrossbowSpriteNames.Add(spriteName);
                    break;
                case ItemGroup.Dagger:
                    SpriteStore.Instance.DaggerSpriteNames.Add(spriteName);
                    break;
                case ItemGroup.OneHandedSpear:
                case ItemGroup.TwoHandedSpear:
                    SpriteStore.Instance.SpearSpriteNames.Add(spriteName);
                    break;
                case ItemGroup.Book:
                    //todo
                    break;
                case ItemGroup.OneHandedSword:
                case ItemGroup.TwoHandedSword:
                    SpriteStore.Instance.SwordSpriteNames.Add(spriteName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@group), @group, null);
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
