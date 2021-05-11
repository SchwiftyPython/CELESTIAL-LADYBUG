using System;
using System.Collections.Generic;
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

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
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
