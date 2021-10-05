using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class MassiveDamage : Ability, IModifierProvider
    {
        private const int DamageMod = 5;

        public MassiveDamage(Entity abilityOwner) : base("Massive Damage", $"+{DamageMod}% damage", -1, -1, abilityOwner, TargetType.Friendly, true)
        {
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(CombatModifierTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes statType))
            {
                return 0f;
            }

            if (statType == CombatModifierTypes.Damage)
            {
                return DamageMod;
            }

            return 0f;
        }
    }
}
