using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Tank : Ability, IModifierProvider
    {
        private const float MaxHealthMod = 20f;

        public Tank(Entity abilityOwner) : base("Tank", $"Adds {MaxHealthMod} to Max Health" ,-1, -1, abilityOwner, false, true)
        {
            Icon = SpriteStore.Instance.GetAbilitySprite(this);
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out EntityStatTypes statType))
            {
                yield return 0f;
            }

            if (statType == EntityStatTypes.MaxHealth)
            {
                yield return MaxHealthMod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}
