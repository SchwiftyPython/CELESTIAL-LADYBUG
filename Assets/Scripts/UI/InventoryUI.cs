using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUi : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.InventoryUpdated;

        [SerializeField] private InventorySlotUi _inventoryItemPrefab = null;

        private Inventory _partyInventory;

        private void Awake()
        {
            _partyInventory = Inventory.GetPartyInventory();
            EventMediator.Instance.SubscribeToEvent(RefreshEvent, this);
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

            if (_partyInventory == null)
            {
                _partyInventory = Inventory.GetPartyInventory();
            }

            for (var i = 0; i < _partyInventory.GetSize(); i++)
            {
                var itemUi = Instantiate(_inventoryItemPrefab, transform);
                itemUi.Setup(_partyInventory, i);
            }
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(RefreshEvent))
            {
                Redraw();
            }
        }
    }
}
