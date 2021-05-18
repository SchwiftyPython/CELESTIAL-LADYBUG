using System;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Cumbersome : Ability, IModifierProvider
    {
        private const int ApMod = -1; 

        public Cumbersome(Entity abilityOwner) : base("Cumbersome", $"{ApMod} Max AP", -1, -1, abilityOwner, false, true)
        {
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out EntityStatTypes statType))
            {
                yield return 0f;
            }

            if (statType == EntityStatTypes.MaxActionPoints)
            {
                yield return ApMod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}
