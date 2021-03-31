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

        private void Awake() 
        {
            //var player = GameObject.FindGameObjectWithTag("Player");
            //playerEquipment = player.GetComponent<Equipment>();
            //_companionEquipment.EquipmentUpdated += RedrawUi;
            EventMediator.Instance.SubscribeToEvent(RefreshEvent, this);
            EventMediator.Instance.SubscribeToEvent(PopulateEvent, this);
        }

        private void Start() 
        {
            //RedrawUi();
        }

        public int MaxAcceptable(Item item)
        {
            if (!(item is EquipableItem equipableItem))
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
            _companionEquipment.AddItem(_equipLocation, (EquipableItem) item);
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

        public void RemoveItems(int number)
        {
            _companionEquipment.RemoveItem(_equipLocation);
        }

        private void RedrawUi()
        {
            var item = GetItem();

            _itemIcon.SetItem(item);

            _slotIcon.GetComponent<Image>().enabled = item == null;
        }

        //todo subscribe to companion change event in party management window
        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PopulateEvent))
            {
                if (!(parameter is Entity companion) || companion.GetEquipment() == null)
                {
                    return;
                }

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