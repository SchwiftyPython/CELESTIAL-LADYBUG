﻿using Assets.Scripts.Items;
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
        [SerializeField] private GameObject _textContainer = null;
        [SerializeField] private TextMeshProUGUI _itemNumber = null;
        
        public void SetItem(Item item)
        {
            SetItem(item, 0);
        }

        public void SetItem(Item item, int number)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
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