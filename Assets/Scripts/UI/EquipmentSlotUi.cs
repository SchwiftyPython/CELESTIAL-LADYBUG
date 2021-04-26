using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using Assets.Scripts.Utilities.UI.Dragging;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// A slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUi : MonoBehaviour, IItemHolder, IDragContainer<Item>, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.EquipmentUpdated;
        private const string PopulateEvent = GlobalHelper.PopulateCharacterSheet;

        [SerializeField] private InventoryItemIcon _itemIcon = null;
        [SerializeField] private GameObject _slotIcon = null;
        [SerializeField] private EquipLocation _equipLocation = EquipLocation.Weapon;

        private Equipment _companionEquipment;
        private Entity _currentCompanion;

        private void Awake() 
        {
            EventMediator.Instance.SubscribeToEvent(RefreshEvent, this);
            EventMediator.Instance.SubscribeToEvent(PopulateEvent, this);
        }

        public int MaxAcceptable(Item item)
        {
            if (!(item is EquipableItem equipableItem))
            {
                return 0;
            }

            if (!_companionEquipment.ItemValidForEntityClass(equipableItem))
            {
                return 0;
            }

            if (equipableItem.GetAllowedEquipLocation() != _equipLocation)
            {
                return 0;
            }

            if (GetItem() != null)
            {
                return 0;
            }

            return 1;
        }

        public void AddItems(Item item, int number)
        {
            _currentCompanion.Equip((EquipableItem) item);
        }

        public Item GetItem()
        {
            return _companionEquipment.GetItemInSlot(_equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void RemoveItems(int number, bool swapAttempt)
        {
            _currentCompanion.UnEquip(_equipLocation, swapAttempt);
        }

        private void RedrawUi()
        {
            if (_companionEquipment.HasSlot(_equipLocation))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }

            var item = GetItem();

            _itemIcon.SetItem(item, _currentCompanion);

            _slotIcon.GetComponent<Image>().enabled = item == null;
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PopulateEvent))
            {
                if (!(parameter is Entity companion) || companion.GetEquipment() == null)
                {
                    return;
                }

                _currentCompanion = companion;
                _companionEquipment = companion.GetEquipment();

                RedrawUi();
            }
            else if (eventName.Equals(RefreshEvent))
            {
                RedrawUi();
            }
        }
    }
}