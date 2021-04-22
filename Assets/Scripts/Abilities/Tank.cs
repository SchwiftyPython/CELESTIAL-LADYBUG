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

        public IEnumerable<float> GetAdditiveModifiers<T>(T stat)
        {
            if ((EntityStatTypes)(object)stat == EntityStatTypes.MaxHealth)
            {
                yield return MaxHealthMod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers<T>(T stat)
        {
            yield return 0f;
        }
    }
}
