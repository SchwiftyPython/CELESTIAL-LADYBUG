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

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out EntityStatTypes statType))
            {
                yield return 0f;
            }

            if (statType == EntityStatTypes.MaxActionPoints)
            {
                yield return MaxApMod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}
