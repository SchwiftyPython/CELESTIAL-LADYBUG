using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class QuickRecovery : Ability, IModifierProvider
    {
        private const int HealBoost = 4;

        public QuickRecovery(Entity abilityOwner) : base("Quick Recovery", $"Boosts hit points recovered when healing by {HealBoost}%.", -1, -1, abilityOwner, TargetType.Hostile, true)
        {
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(EntityStatTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out EntityStatTypes statType))
            {
                return 0f;
            }

            if (statType == EntityStatTypes.CurrentHealth)
            {
                return HealBoost;
            }

            return 0f;
        }
    }
}
