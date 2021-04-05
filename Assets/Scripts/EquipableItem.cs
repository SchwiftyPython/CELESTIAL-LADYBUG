using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("Equipable Item"))]
    public class EquipableItem : Item
    {
        private EquipLocation _allowedEquipLocation;

        public EquipableItem(EquipLocation equipLocation, string itemName, string description, Sprite icon, bool stackable) : base()
        {
            _allowedEquipLocation = equipLocation;
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            return _allowedEquipLocation;
        }
    }
}