using Assets.Scripts.Items;
using Assets.Scripts.Utilities.UI.Dragging;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// A slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUi : MonoBehaviour, IItemHolder, IDragContainer<Item>
    {
        [SerializeField] private InventoryItemIcon _icon = null;
        [SerializeField] private EquipLocation _equipLocation = EquipLocation.Weapon;

        private Equipment _companionEquipment;

        private void Awake() 
        {
            //var player = GameObject.FindGameObjectWithTag("Player");
            //playerEquipment = player.GetComponent<Equipment>();
            _companionEquipment.EquipmentUpdated += RedrawUi;
        }

        private void Start() 
        {
            RedrawUi();
        }

        public int MaxAcceptable(Item item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null)
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

        Item IDragSource<Item>.GetItem()
        {
            throw new System.NotImplementedException();
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
            _icon.SetItem(_companionEquipment.GetItemInSlot(_equipLocation));
        }

        //todo subscribe to companion change event in party management window
    }
}