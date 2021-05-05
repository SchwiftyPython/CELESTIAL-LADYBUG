using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class UncannyDodge : Ability, IModifierProvider
    {
        private const int DodgeMod = 2;

        public UncannyDodge(Entity abilityOwner) : base("Uncanny Dodge", $"+{DodgeMod} to dodge.", -1, -1, abilityOwner, false, true)
        {
            Icon = SpriteStore.Instance.GetAbilitySprite(this);
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum stat)
        {
            if (!Enum.TryParse(stat.ToString(), out EntitySkillTypes skillType))
            {
                yield return 0f;
            }

            if (skillType == EntitySkillTypes.Dodge)
            {
                yield return DodgeMod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum stat)
        {
            yield return 0f;
        }
    }
}
