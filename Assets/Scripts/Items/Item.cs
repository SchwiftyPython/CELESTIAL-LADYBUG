using System.ComponentModel;
using GoRogue;
using GoRogue.GameFramework;
using GameObject = UnityEngine.GameObject;

namespace Assets.Scripts.Items
{
    //todo inherit from GameObject or IGameobject to take advantage of components?
    public class Item
    {
        public enum ItemType
        {
            [Description("Sword")]
            Sword,
            [Description("Leather Armor")]
            LeatherArmor
        }

        public GameObject InventoryPrefab { get; private set; }
        public GameObject InventorySprite { get; private set; }

        public string ItemName { get; private set; }

        public ItemType Type { get; private set; }

        public Item(string itemName, ItemType type)
        {
            ItemName = itemName;
            Type = type;
        }
    }
}
