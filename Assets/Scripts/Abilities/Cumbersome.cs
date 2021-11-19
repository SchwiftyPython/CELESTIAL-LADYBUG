using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Cumbersome : Ability, IModifierProvider
    {
        private const int ApMod = -1; 

        public Cumbersome(Entity abilityOwner) : base("Cumbersome", $"{ApMod} Max AP", -1, -1, abilityOwner, TargetType.Friendly, true)
        {
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(EntityStatTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out EntityStatTypes statType))
            {
                return 0f;
            }

            if (statType == EntityStatTypes.MaxActionPoints)
            {
                return ApMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
