using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Armor : EquipableItem
    {
        public int Toughness { get; private set; }

        public Armor(EquipLocation equipLocation, string itemName, ItemType type, int toughness, string description, Sprite icon, bool stackable) : base(equipLocation, itemName, type, description, icon, stackable)
        {
            Toughness = toughness;
        }
    }
}
