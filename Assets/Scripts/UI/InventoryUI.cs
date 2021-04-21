using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// To be placed on the root of the inventory UI. Handles spawning all the
    /// inventory slot prefabs.
    /// </summary>
    public class InventoryUi : MonoBehaviour, ISubscriber
    {
        private readonly List<string> _refreshEvents = new List<string>
            {GlobalHelper.InventoryUpdated, GlobalHelper.PopulateCharacterSheet};

        [SerializeField] private InventorySlotUi _inventoryItemPrefab = null;

        private Inventory _partyInventory;

        private Entity _currentEntity;

        private void Awake()
        {
            _partyInventory = Inventory.GetPartyInventory();

            foreach (var eventName in _refreshEvents)
            {
                EventMediator.Instance.SubscribeToEvent(eventName, this);
            }
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
                itemUi.Setup(_partyInventory, i, _currentEntity);
            }
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (_refreshEvents.Contains(eventName))
            {
                if (parameter is Entity companion)
                {
                    _currentEntity = companion;
                }

                Redraw();
            }
        }
    }
}
