using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.Items
{
    //todo inherit from GameObject or IGameobject to take advantage of components?
    public class Item : ScriptableObject, ISerializationCallbackReceiver
    {
        public enum ItemType
        {
            [Description("Sword")]
            Sword,
            [Description("Leather Armor")]
            LeatherArmor
        }
        
        [SerializeField] private string _itemId = null;
        [SerializeField] private string _displayName = null;
        [SerializeField] private string _description = null;
        [SerializeField] private Sprite _icon = null;
        [SerializeField] private bool _stackable = false;

        public ItemType Type { get; private set; }

        public static Dictionary<string, Item> ItemLookupCache;

        public Item(string itemName, ItemType type, string description, Sprite icon, bool stackable)
        {
            _displayName = itemName;
            Type = type;
            _description = description;
            _icon = icon;
            _stackable = stackable;
        }

        /// <summary>
        /// Get the inventory item instance from its GUID.
        /// </summary>
        /// <param name="itemId">
        /// String GUID that persists between game instances.
        /// </param>
        /// <returns>
        /// Inventory item instance corresponding to the ID.
        /// </returns>
        public static Item GetFromId(string itemId)
        {
            if (ItemLookupCache == null)
            {
                ItemLookupCache = new Dictionary<string, Item>();
                var itemList = Resources.LoadAll<Item>("");
                foreach (var item in itemList)
                {
                    if (ItemLookupCache.ContainsKey(item._itemId))
                    {
                        Debug.LogError(
                            $"Looks like there's a duplicate InventorySystem ID for objects: {ItemLookupCache[item._itemId]} and {item}");
                        continue;
                    }

                    ItemLookupCache[item._itemId] = item;
                }
            }

            return itemId == null || !ItemLookupCache.ContainsKey(itemId) ? null : ItemLookupCache[itemId];
        }

        public Sprite GetIcon()
        {
            return _icon;
        }

        public string GetItemId()
        {
            return _itemId;
        }

        public bool IsStackable()
        {
            return _stackable;
        }

        public string GetDisplayName()
        {
            return _displayName;
        }

        public string GetDescription()
        {
            return _description;
        }

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(_itemId))
            {
                _itemId = System.Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }
    }
}
