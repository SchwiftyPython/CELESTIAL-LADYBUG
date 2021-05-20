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

        public float GetAdditiveModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(EntitySkillTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out EntitySkillTypes skillType))
            {
                return 0f;
            }

            if (skillType == EntitySkillTypes.Dodge)
            {
                return DodgeMod;
            }

            return 0;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
