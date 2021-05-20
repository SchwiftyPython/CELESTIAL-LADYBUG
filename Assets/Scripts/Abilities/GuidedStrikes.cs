using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class GuidedStrikes : Ability, IModifierProvider
    {
        private const int ToHitBonus = 3;
    
        public GuidedStrikes(Entity abilityOwner) : base("Guided Strikes", $"+{ToHitBonus} to hit on melee attacks.", -1, -1, abilityOwner, false, true)
        {
        }

        public static int GetToHitBonus()
        {
            return ToHitBonus;
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

            if (statType == CombatModifierTypes.MeleeToHit)
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
