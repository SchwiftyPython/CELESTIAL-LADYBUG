using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Weapon : EquipableItem
    {
        public (int, int) DamageRange { get; private set; }
        public int Range { get; private set; }
        public bool IsRanged { get; private set; }

        public Weapon(string itemName, ItemType type, (int, int) damageRange, int range, string description, Sprite icon, bool stackable) : base(EquipLocation.Weapon, itemName, type, description, icon, stackable)
        {
            DamageRange = damageRange;
            Range = range;
            IsRanged = Range > 1;
        }
    }
}
