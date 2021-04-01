﻿using Assets.Scripts.Utilities.Tooltips;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// To be placed on a UI slot to spawn and show the correct item tooltip.
    /// </summary>
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            var item = GetComponent<IItemHolder>().GetItem();
            return item != null;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            if (!itemTooltip)
            {
                return;
            }

            var item = GetComponent<IItemHolder>().GetItem();

            itemTooltip.Setup(item);
        }
    }
}