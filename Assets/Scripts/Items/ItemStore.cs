using System;
using System.Collections.Generic;
using Assets.Scripts.Items.Components;
using Leguar.TotalJSON;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemStore : MonoBehaviour
    {
        private Dictionary<string, ItemType> _itemTypes;

        public TextAsset ItemTypesFile;

        public static ItemStore Instance;

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

            DeserializeItemTypes();
        }

        public ItemType GetItemTypeByName(string typeName)
        {
            if (typeName == null || !_itemTypes.ContainsKey(typeName.ToLower()))
            {
                return null;
            }

            return _itemTypes[typeName.ToLower()];
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

                itemType.Parent = itemTypeJson.GetString("Parent")?.Trim();

                itemType.Melee = itemTypeJson.GetJSON("Melee").Deserialize<Attack>(settings);

                itemType.Ranged = itemTypeJson.GetJSON("Ranged").Deserialize<Attack>(settings);

                itemType.Defense = itemTypeJson.GetJSON("Defense").Deserialize<Defense>(settings);

                itemType.Abilities = new List<string>(itemTypeJson.GetJArray("Abilities").AsStringArray());

                itemType.Sprite = itemTypeJson.GetString("Sprite")?.Trim();

                var slotString = itemTypeJson.GetString("Slot")?.Trim();

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
    }
}
