using Assets.Scripts.Items;
using Assets.Scripts.Utilities.UI.Dragging;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InventorySlotUi : MonoBehaviour, IItemHolder, IDragContainer<Item>
    {
        [SerializeField] private InventoryItemIcon _icon = null;

        private int _index;
        private Item _item;
        private Inventory _inventory;

        public void Setup(Inventory inventory, int index)
        {
            _inventory = inventory;
            _index = index;
            _icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(Item item)
        {
            if (_inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(Item item, int number)
        {
            _inventory.AddItemToSlot(_index, item, number);
        }

        public Item GetItem()
        {
            return _inventory.GetItemInSlot(_index);
        }

        public int GetNumber()
        {
            return _inventory.GetNumberInSlot(_index);
        }

        public void RemoveItems(int number)
        {
            _inventory.RemoveFromSlot(_index, number);
        }
    }
}
