using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Tank : Ability, IModifierProvider
    {
        private const float MaxHealthMod = 20f;

        public Tank(Entity abilityOwner) : base("Tank", $"+{MaxHealthMod} Max Health" ,-1, -1, abilityOwner, TargetType.Friendly, true)
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

            if (statType == EntityStatTypes.MaxHealth)
            {
                return MaxHealthMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
