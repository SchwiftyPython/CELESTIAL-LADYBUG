using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Snapshot : Ability, IModifierProvider
    {
        private const int ToHitMod = -4;

        public Snapshot(Entity abilityOwner) : base("Snapshot", $"Take a quick shot at the cost of accuracy.\n{ToHitMod}% chance to hit", 4, 5, abilityOwner, TargetType.Friendly, false)
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
                return ToHitMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
