using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using Assets.Scripts.Saving;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    /// <summary>
    /// Provides a store for the items equipped to an entity. Items are stored by
    /// their equip locations.  
    /// </summary>
    [Serializable]
    public class Equipment : ISaveable
    {
        private readonly Dictionary<EntityClass, List<ItemGroup>> _allowedItemGroupsByClass =
            new Dictionary<EntityClass, List<ItemGroup>>
            {
                {EntityClass.Spearman, new List<ItemGroup> {ItemGroup.OneHandedSpear, ItemGroup.TwoHandedSpear, ItemGroup.Armor, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}},
                {EntityClass.Crossbowman, new List<ItemGroup> {ItemGroup.Crossbow, ItemGroup.Armor, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}},
                {EntityClass.ManAtArms, new List<ItemGroup> {ItemGroup.OneHandedSword, ItemGroup.Shield, ItemGroup.Armor, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}},
                {EntityClass.Gladiator, new List<ItemGroup> {ItemGroup.OneHandedSword, ItemGroup.Shield, ItemGroup.Armor, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}},
                {EntityClass.Wizard, new List<ItemGroup> {ItemGroup.Book, ItemGroup.Robe, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}},
                {EntityClass.BattleMage, new List<ItemGroup> {ItemGroup.OneHandedSword, ItemGroup.Book, ItemGroup.Armor, ItemGroup.Robe, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}},
                {EntityClass.Knight, new List<ItemGroup> {ItemGroup.TwoHandedSword, ItemGroup.Armor, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}},
                {EntityClass.Paladin, new List<ItemGroup> {ItemGroup.TwoHandedSword, ItemGroup.Armor, ItemGroup.Feet, ItemGroup.Glove, ItemGroup.Helmet, ItemGroup.Ring}}
            };

        [ES3Serializable]
        private Dictionary<EquipLocation, EquipableItem> _equippedItems;

        [ES3Serializable]
        private EntityClass _entityClass;

        public Equipment()
        {
        }

        public Equipment(EntityClass entityClass)
        {
            _entityClass = entityClass;

            _equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            if (!_allowedItemGroupsByClass.ContainsKey(entityClass))
            {
                return;
            }

            foreach (var group in _allowedItemGroupsByClass[_entityClass])
            {
                switch (group)
                {
                    case ItemGroup.Robe:
                    case ItemGroup.Armor:
                        if (_equippedItems.ContainsKey(EquipLocation.Body))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Body, null);
                        break;
                    case ItemGroup.Feet:
                        if (_equippedItems.ContainsKey(EquipLocation.Boots))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Boots, null);
                        break;
                    case ItemGroup.Glove:
                        if (_equippedItems.ContainsKey(EquipLocation.Gloves))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Gloves, null);
                        break;
                    case ItemGroup.Helmet:
                        if (_equippedItems.ContainsKey(EquipLocation.Helmet))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Helmet, null);
                        break;
                    case ItemGroup.Ring:
                        if (_equippedItems.ContainsKey(EquipLocation.Ring))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Ring, null);
                        break;
                    case ItemGroup.Shield:
                        if (_equippedItems.ContainsKey(EquipLocation.Shield))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Shield, null);
                        break;
                    case ItemGroup.Axe:
                    case ItemGroup.Crossbow:
                    case ItemGroup.Dagger:
                    case ItemGroup.OneHandedSpear:
                    case ItemGroup.TwoHandedSpear:
                    case ItemGroup.OneHandedSword:
                    case ItemGroup.TwoHandedSword:
                        if (_equippedItems.ContainsKey(EquipLocation.Weapon))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Weapon, null);
                        break;
                    case ItemGroup.Book:
                        if (_equippedItems.ContainsKey(EquipLocation.Book))
                        {
                            continue;
                        }
                        _equippedItems.Add(EquipLocation.Book, null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool HasSlot(EquipLocation equipLocation)
        {
            return _equippedItems.ContainsKey(equipLocation);
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
            if (_equippedItems == null || !_equippedItems.ContainsKey(slot))
            {
                return;
            }

            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            _equippedItems[slot] = item;

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EquipmentUpdated, this);
        }

        /// <summary>
        /// Remove the item for the given slot.
        /// </summary>
        public void RemoveItem(EquipLocation slot)
        {
            if (!_equippedItems.ContainsKey(slot))
            {
                return;
            }

            _equippedItems[slot] = null;
            //eventMediator.Broadcast(GlobalHelper.EquipmentUpdated, this);
        }

        public void RemoveAllItems()
        {
            foreach (var slot in _equippedItems.Keys.ToArray())
            {
                RemoveItem(slot);
            }
        }

        public bool AbilityEquipped(Ability ability)
        {
            foreach (var item in _equippedItems.Values)
            {
                if (item == null)
                {
                    continue;
                }

                var itemAbilities = item.GetAbilities(null);

                foreach (var itemAbility in itemAbilities)
                {
                    if (itemAbility.GetType() == ability.GetType())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ItemValidForEntityClass(EquipableItem item)
        {
            if (_entityClass == EntityClass.Derpus || !_allowedItemGroupsByClass.ContainsKey(_entityClass))
            {
                return false;
            }

            var allowedItemGroups = _allowedItemGroupsByClass[_entityClass];

            return allowedItemGroups.Contains(item.GetItemGroup());
        }

        public int GetTotalArmorToughness()
        {
            var toughnessTotal = 0;

            foreach (EquipLocation location in Enum.GetValues(typeof(EquipLocation)))
            {
                if (location == EquipLocation.Weapon)
                {
                    continue;
                }

                var equippedArmor = GetItemInSlot(location);

                if (equippedArmor == null)
                {
                    continue;
                }

                toughnessTotal += equippedArmor.GetToughness();
            }

            return toughnessTotal;
        }

        public int GetDodgeTotal()
        {
            var dodgeTotal = 0;

            foreach (EquipLocation location in Enum.GetValues(typeof(EquipLocation)))
            {
                var equippedItem = GetItemInSlot(location);

                if (equippedItem == null)
                {
                    continue;
                }

                dodgeTotal += equippedItem.GetDodgeMod();
            }

            return dodgeTotal;
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
                // var item = (EquipableItem)Item.GetFromId(pair.Value);
                // if (item != null)
                // {
                //     _equippedItems[pair.Key] = item;
                // }
            }
        }
    }
}