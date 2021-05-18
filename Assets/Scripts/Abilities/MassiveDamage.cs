using System;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class MassiveDamage : Ability, IModifierProvider
    {
        private const int DamageMod = 5;

        public MassiveDamage(Entity abilityOwner) : base("Massive Damage", $"+{DamageMod}% damage", -1, -1, abilityOwner, false, true)
        {
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            yield return 0f;
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes statType))
            {
                yield return 0f;
            }

            if (statType == CombatModifierTypes.Damage)
            {
                yield return DamageMod;
            }
        }
    }
}
