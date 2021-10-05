using System;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Calculated : Ability
    {
        private const int ToHitBonus = 3;

        public Calculated(Entity abilityOwner) : base("Calculated", $"+{ToHitBonus} to hit on ranged attacks.", -1, -1, abilityOwner, TargetType.Friendly, true)
        {
        }

        public static int GetToHitBonus()
        {
            return ToHitBonus;
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(CombatModifierTypes)))
            {
                yield return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes statType))
            {
                yield return 0f;
            }

            if (statType == CombatModifierTypes.RangedToHit)
            {
                yield return ToHitBonus;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}
