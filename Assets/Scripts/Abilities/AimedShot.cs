using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class AimedShot : Ability, IModifierProvider
    {
        private const int ToHitBonus = 10;

        public AimedShot(Entity abilityOwner) : base("Aimed Shot", $"+{ToHitBonus}% to hit.", 7, 7, abilityOwner, true,
            false)
        {
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(CombatModifierTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes statType))
            {
                return 0f;
            }

            if (statType == CombatModifierTypes.RangedToHit)
            {
                return ToHitBonus;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
