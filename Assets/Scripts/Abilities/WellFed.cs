using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Abilities
{
    public class WellFed : Ability, IModifierProvider
    {
        private const int MaxApMod = 1;

        public WellFed(Entity abilityOwner) : base("Well Fed", $"+{MaxApMod} Max Action Points", -1, -1, abilityOwner, false, true)
        {
            var spriteStore = Object.FindObjectOfType<SpriteStore>();
            Icon = spriteStore.GetAbilitySprite(this);
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
                return MaxApMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
