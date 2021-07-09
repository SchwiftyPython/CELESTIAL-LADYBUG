using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// To be put on the icon representing an inventory item. Allows the slot to
    /// update the icon and number.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        private Color _enabledColor = Color.white;
        private Color _disabledColor = new Color32((byte) 69f, (byte)39f, (byte)49f, byte.MaxValue / 3);

        [SerializeField] private GameObject _textContainer = null;
        [SerializeField] private TextMeshProUGUI _itemNumber = null;

        private Entity _currentCompanion;
        
        public void SetItem(Item item, Entity currentCompanion)
        {
            SetItem(item, 1, currentCompanion);
        }

        public void SetItem(Item item, int number, Entity currentCompanion)
        {
            _currentCompanion = currentCompanion;

            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                if (_currentCompanion != null && _currentCompanion.GetEquipment().ItemValidForEntityClass((EquipableItem) item))
                {
                    iconImage.color = _enabledColor;
                }
                else
                {
                    iconImage.color = _disabledColor;
                }

                iconImage.enabled = true;
                iconImage.sprite = item.GetIcon();
            }

            if (_itemNumber)
            {
                if (number <= 1)
                {
                    _textContainer.SetActive(false);
                }
                else
                {
                    _textContainer.SetActive(true);
                    _itemNumber.text = number.ToString();
                }
            }
        }
    }
}