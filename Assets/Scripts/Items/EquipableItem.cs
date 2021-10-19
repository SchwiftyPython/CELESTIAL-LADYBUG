using System;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// An item that can be equipped to the player.
    /// </summary>
    public class EquipableItem : Item
    {
        public EquipableItem()
        {
        }

        public EquipableItem(ItemType itemType) : base(itemType)
        {
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            if (ItemType.Slot == null)
            {
                ItemStore itemStore = Object.FindObjectOfType<ItemStore>();

                ItemType.Slot = itemStore.GetItemTypeByName(GetName()).Slot;
            }

            if (ItemType.Slot != null)
            {
                return (EquipLocation)ItemType.Slot;
            }

            throw new Exception($"Equipment location slot for {ItemType.Name} is null!");
        }

        public int GetRange()
        {
            return ItemType.Range;
        }

        public (int, int) GetMeleeDamageRange()
        {
            if (ItemType.Melee == null)
            {
                return (0, 0);
            }

            return (ItemType.Melee.MinDamage, ItemType.Melee.MaxDamage);
        }

        public (int, int) GetRangedDamageRange()
        {
            if (ItemType.Ranged == null)
            {
                return (0, 0);
            }

            return (ItemType.Ranged.MinDamage, ItemType.Ranged.MaxDamage);
        }

        public int GetPrice()
        {
            return ItemType.Price;
        }
    }
}