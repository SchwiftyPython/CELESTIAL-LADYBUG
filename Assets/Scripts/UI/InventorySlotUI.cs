using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<Item>
    {
        [SerializeField] InventoryItemIcon icon = null;

        int index;
        Item item;
        Inventory inventory;

        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(Item item)
        {
            if (inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(Item item, int number)
        {
            inventory.AddItemToSlot(index, item, number);
        }

        public Item GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }
    }
}
