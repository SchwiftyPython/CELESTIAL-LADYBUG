using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Abilities
{
    public class UncannyDodge : Ability, IModifierProvider
    {
        private const int DodgeMod = 2;

        public UncannyDodge(Entity abilityOwner) : base("Uncanny Dodge", $"+{DodgeMod} to dodge.", -1, -1, abilityOwner, false, true)
        {
            var spriteStore = Object.FindObjectOfType<SpriteStore>();
            Icon = spriteStore.GetAbilitySprite(this);
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
