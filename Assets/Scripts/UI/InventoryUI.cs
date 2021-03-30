using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] private InventorySlotUi _inventoryItemPrefab = null;

        private Inventory _playerInventory;

        private void Awake()
        {
            _playerInventory = Inventory.GetPartyInventory();
            _playerInventory.InventoryUpdated += Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (var i = 0; i < _playerInventory.GetSize(); i++)
            {
                var itemUi = Instantiate(_inventoryItemPrefab, transform);
                itemUi.Setup(_playerInventory, i);
            }
        }
    }
}
