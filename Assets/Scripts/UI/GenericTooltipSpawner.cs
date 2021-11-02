using Assets.Scripts.Utilities.Tooltips;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GenericTooltipSpawner : TooltipSpawner
    {
        [SerializeField] private string tooltipText;

        public override void UpdateTooltip(GameObject tooltip)
        {
            var genericTooltip = tooltip.GetComponent<GenericToolTip>();

            if (!genericTooltip)
            {
                return;
            }

            genericTooltip.Setup(tooltipText);
        }

        public override bool CanCreateTooltip()
        {
            return !string.IsNullOrWhiteSpace(tooltipText);
        }
    }
}
