using System;
using Assets.Scripts.Items;
using Assets.Scripts.Saving;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Provides storage for the player inventory. A configurable number of
    /// slots are available.
    ///
    /// This component should be placed on the GameObject tagged "TravelManager".
    /// </summary>
    public class Inventory : MonoBehaviour, ISaveable
    {
        [Tooltip("Allowed Size")]
        [SerializeField]
        private int _inventorySize = 45;

        private InventorySlot[] _slots;

        public struct InventorySlot
        {
            public Item Item;
            public int Number;
        }

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action InventoryUpdated;

        /// <summary>
        /// Convenience for getting the player's inventory.
        /// </summary>
        public static Inventory GetPartyInventory()
        {
            var travelManager = GameObject.FindWithTag("TravelManager");
            return travelManager.GetComponent<Inventory>();
        }

        /// <summary>
        /// Could this item fit anywhere in the inventory?
        /// </summary>
        public bool HasSpaceFor(Item item)
        {
            return FindSlot(item) >= 0;
        }

        /// <summary>
        /// How many slots are in the inventory?
        /// </summary>
        public int GetSize()
        {
            return _slots.Length;
        }

        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="number">The number to add.</param>
        /// <returns>Whether or not the item could be added.</returns>
        public bool AddToFirstEmptySlot(Item item, int number)
        {
            var i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            _slots[i].Item = item;
            _slots[i].Number += number;
            InventoryUpdated?.Invoke();
            return true;
        }

        /// <summary>
        /// Is there an instance of the item in the inventory?
        /// </summary>
        public bool HasItem(Item item)
        {
            for (var i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i].Item, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the item in the given slot.
        /// </summary>
        public Item GetItemInSlot(int slot)
        {
            return _slots[slot].Item;
        }

        /// <summary>
        /// Get the number of items in the given slot.
        /// </summary>
        public int GetNumberInSlot(int slot)
        {
            return _slots[slot].Number;
        }

        /// <summary>
        /// Remove a number of items from the given slot. Will never remove more
        /// than there are.
        /// </summary>
        public void RemoveFromSlot(int slot, int number)
        {
            _slots[slot].Number -= number;
            if (_slots[slot].Number <= 0)
            {
                _slots[slot].Number = 0;
                _slots[slot].Item = null;
            }

            InventoryUpdated?.Invoke();
        }

        /// <summary>
        /// Will add an item to the given slot if possible. If there is already
        /// a stack of this type, it will add to the existing stack. Otherwise,
        /// it will be added to the first empty slot.
        /// </summary>
        /// <param name="slot">The slot to attempt to add to.</param>
        /// <param name="item">The item type to add.</param>
        /// <param name="number">The number of items to add.</param>
        /// <returns>True if the item was added anywhere in the inventory.</returns>
        public bool AddItemToSlot(int slot, Item item, int number)
        {
            if (_slots[slot].Item != null)
            {
                return AddToFirstEmptySlot(item, number);
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            _slots[slot].Item = item;
            _slots[slot].Number += number;
            InventoryUpdated?.Invoke();
            return true;
        }

        private void Awake()
        {
            _slots = new InventorySlot[_inventorySize];
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        private int FindSlot(Item item)
        {
            var i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        private int FindEmptySlot()
        {
            for (var i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        private int FindStack(Item item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (var i = 0; i < _slots.Length; i++)
            {
                if (ReferenceEquals(_slots[i].Item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        [Serializable]
        private struct InventorySlotRecord
        {
            public string ItemId;
            public int Number;
        }

        object ISaveable.CaptureState()
        {
            var slotStrings = new InventorySlotRecord[_inventorySize];
            for (var i = 0; i < _inventorySize; i++)
            {
                if (_slots[i].Item != null)
                {
                    slotStrings[i].ItemId = _slots[i].Item.GetItemId();
                    slotStrings[i].Number = _slots[i].Number;
                }
            }
            return slotStrings;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (var i = 0; i < _inventorySize; i++)
            {
                _slots[i].Item = Item.GetFromId(slotStrings[i].ItemId);
                _slots[i].Number = slotStrings[i].Number;
            }

            InventoryUpdated?.Invoke();
        }
    }
}
