using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class WorksOut : Ability, IModifierProvider
    {
        private const int PhysiqueMod = 1;

        public WorksOut(Entity abilityOwner) : base("Works Out", $"+{PhysiqueMod} to Physique", -1, -1, abilityOwner,
            false, true)
        {
        }

        public IEnumerable<float> GetAdditiveModifiers(Enum attribute)
        {
            if (!Enum.TryParse(attribute.ToString(), out EntityAttributeTypes attributeType))
            {
                yield return 0f;
            }

            if (attributeType == EntityAttributeTypes.Physique)
            {
                yield return PhysiqueMod;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Enum attribute)
        {
            yield return 0f;
        }
    }
}
