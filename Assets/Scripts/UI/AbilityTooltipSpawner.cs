using Assets.Scripts.Utilities.Tooltips;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(IAbilityHolder))]
    public class AbilityTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var abilityTooltip = tooltip.GetComponent<AbilityTooltip>();
            if (!abilityTooltip)
            {
                return;
            }

            var ability = GetComponent<IAbilityHolder>().GetAbility();

            abilityTooltip.Setup(ability);
        }

        public override bool CanCreateTooltip()
        {
            var ability = GetComponent<IAbilityHolder>().GetAbility();
            return ability != null;
        }
    }
}
