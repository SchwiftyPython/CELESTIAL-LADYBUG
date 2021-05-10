using System;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Snapshot : Ability, IModifierProvider
    {
        private const int ToHitMod = -4;

        public Snapshot(Entity abilityOwner) : base("Snapshot", $"Take a quick shot at the cost of accuracy.\n{ToHitMod}% chance to hit", 4, 5, abilityOwner, true, false)
        {
        }

        public static int GetToHitMod()
        {
            return ToHitMod;
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes statType))
            {
                yield return 0f;
            }

            if (statType == CombatModifierTypes.RangedToHit)
            {
                yield return ToHitMod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}
