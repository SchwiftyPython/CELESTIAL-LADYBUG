using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Knowledgeable : Ability, IModifierProvider
    {
        private const int IntellectMod = 1; 

        public Knowledgeable(Entity abilityOwner) : base("Knowledgeable", $"+{IntellectMod} Intellect", -1, -1, abilityOwner, false, true)
        {
        }

        public float GetAdditiveModifiers(Enum attribute)
        {
            if (!attribute.GetType().Name.Equals(nameof(EntityAttributeTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(attribute.ToString(), out EntityAttributeTypes attributeType))
            {
                return 0f;
            }

            if (attributeType == EntityAttributeTypes.Intellect)
            {
                return IntellectMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum attribute)
        {
            return 0f;
        }
    }
}
