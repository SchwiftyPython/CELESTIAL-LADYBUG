using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Charismatic : Ability, IModifierProvider
    {
        private const int CharismaMod = 1;

        public Charismatic(Entity abilityOwner) : base("Charismatic", $"+{CharismaMod} Charisma.", -1, -1,
            abilityOwner, TargetType.Friendly, true)
        {
        }

        public float GetAdditiveModifiers(Enum skill)
        {
            if (!skill.GetType().Name.Equals(nameof(EntityAttributeTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(skill.ToString(), out EntityAttributeTypes skillType))
            {
                return 0f;
            }

            if (skillType == EntityAttributeTypes.Charisma)
            {
                return CharismaMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            return 0f;
        }
    }
}
