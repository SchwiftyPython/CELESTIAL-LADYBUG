using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class QuickRecovery : Ability, IModifierProvider
    {
        private const int HealBoost = 4;

        public QuickRecovery(Entity abilityOwner) : base("Quick Recovery", $"Boosts hit points recovered when healing by {HealBoost}%.", -1, -1, abilityOwner, false, true)
        {
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            yield return 0f;
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out EntityStatTypes statType))
            {
                yield return 0f;
            }

            if (statType == EntityStatTypes.CurrentHealth)
            {
                yield return HealBoost;
            }
        }
    }
}
