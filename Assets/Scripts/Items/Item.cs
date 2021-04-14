using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Item
    {
        [SerializeField] private string _itemId;

        [SerializeField] protected ItemType ItemType;

        public Item(ItemType itemType)
        {
            ItemType = itemType;
            _itemId = Guid.NewGuid().ToString();
        }

        public Sprite GetIcon()
        {
            return ItemType.Sprite;
        }

        public string GetItemId()
        {
            return _itemId;
        }

        public bool IsStackable()
        {
            return ItemType.Stackable;
        }

        public string GetDisplayName()
        {
            return ItemType.Name;
        }

        public string GetDescription()
        {
            return ItemType.Description;
        }
    }
}
