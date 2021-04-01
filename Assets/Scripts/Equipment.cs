using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Saving;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Provides a store for the items equipped to an entity. Items are stored by
    /// their equip locations.  
    /// </summary>
    public class Equipment : ISaveable
    {
        private Dictionary<EquipLocation, EquipableItem> _equippedItems;

        public Equipment()
        {
            _equippedItems = new Dictionary<EquipLocation, EquipableItem>
            {
                {EquipLocation.Helmet, null},
                {EquipLocation.Gloves, null},
                {EquipLocation.Body, null},
                {EquipLocation.Boots, null},
                {EquipLocation.Ring, null},
                {EquipLocation.Weapon, null},
                {EquipLocation.Shield, null} //only some classes have shield available
            };
        }

        /// <summary>
        /// Return the item in the given equip location.
        /// </summary>
        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!_equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return _equippedItems[equipLocation];
        }

        /// <summary>
        /// Add an item to the given equip location. Do not attempt to equip to
        /// an incompatible slot.
        /// </summary>
        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            _equippedItems[slot] = item;

            EventMediator.Instance.Broadcast(GlobalHelper.EquipmentUpdated, this);
        }

        /// <summary>
        /// Remove the item for the given slot.
        /// </summary>
        public void RemoveItem(EquipLocation slot)
        {
            //_equippedItems.Remove(slot);
            _equippedItems[slot] = null;
            EventMediator.Instance.Broadcast(GlobalHelper.EquipmentUpdated, this);
        }

        object ISaveable.CaptureState()
        {
            //todo 

            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in _equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.GetItemId();
            }
            return equippedItemsForSerialization;
        }

        void ISaveable.RestoreState(object state)
        {
            //todo

            _equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (EquipableItem)Item.GetFromId(pair.Value);
                if (item != null)
                {
                    _equippedItems[pair.Key] = item;
                }
            }
        }
    }
}