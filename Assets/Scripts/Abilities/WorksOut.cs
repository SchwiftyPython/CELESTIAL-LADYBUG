using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class WorksOut : Ability, IModifierProvider
    {
        private const int PhysiqueMod = 1;

        public WorksOut(Entity abilityOwner) : base("Works Out", $"+{PhysiqueMod} Physique", -1, -1, abilityOwner,
            TargetType.Friendly, true)
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

            if (attributeType == EntityAttributeTypes.Physique)
            {
                return PhysiqueMod;
            }

            return 0f;
        }

        public float GetPercentageModifiers(Enum attribute)
        {
            return 0f;
        }
    }
}
