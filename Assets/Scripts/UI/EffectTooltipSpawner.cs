using Assets.Scripts.Utilities.Tooltips;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(IEffectHolder))]
    public class EffectTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var effectTooltip = tooltip.GetComponent<EffectTooltip>();
            if (!effectTooltip)
            {
                return;
            }

            var effect = GetComponent<IEffectHolder>().GetEffect();

            effectTooltip.Setup(effect);
        }

        public override bool CanCreateTooltip()
        {
            var effect = GetComponent<IEffectHolder>().GetEffect();
            return effect != null;
        }
    }
}
