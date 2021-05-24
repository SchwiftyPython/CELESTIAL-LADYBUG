using System;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// An item that can be equipped to the player.
    /// </summary>
    public class EquipableItem : Item
    {
        public EquipableItem(ItemType itemType) : base(itemType)
        {
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            if (ItemType.Slot == null)
            {
                throw new Exception($"Equipment location slot for {ItemType.Name} is null!");
            }

            return (EquipLocation) ItemType.Slot;
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
    }
}