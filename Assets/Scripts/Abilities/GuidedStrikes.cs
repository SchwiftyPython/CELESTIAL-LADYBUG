using System;
using System.Collections.Generic;
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

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes statType))
            {
                yield return 0f;
            }

            if (statType == CombatModifierTypes.MeleeToHit)
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
