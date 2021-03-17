using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Attributes
    {
        private const int AttributeMin = 1;
        private const int MagicAttributeMin = 0;
        private const int AttributeMax = 5;

        private const int StartingAttributeDice = 12;

        //todo need to update Stats when these values change
        public int Agility { get; set; }
        public int Coordination { get; set; }
        public int Physique { get; set; }
        public int Intellect { get; set; }
        public int Acumen { get; set; }
        public int Charisma { get; set; }

        //public int Magic { get; set; } todo not implemented

        public Attributes()
        {
            GenerateAttributeValues();
        }

        private void GenerateAttributeValues()
        {
            var startingValues = new Dictionary<EntityAttributeTypes, int>()
            {
                { EntityAttributeTypes.Agility, AttributeMin },
                { EntityAttributeTypes.Coordination, AttributeMin },
                { EntityAttributeTypes.Physique, AttributeMin },
                { EntityAttributeTypes.Intellect, AttributeMin },
                { EntityAttributeTypes.Acumen, AttributeMin },
                { EntityAttributeTypes.Charisma, AttributeMin }
                //{ "magic", 0 }
            };

            var remainingDice = StartingAttributeDice;

            while (remainingDice > 0)
            {
                var attributeIndex = Random.Range(0, startingValues.Count);

                var attributeKey = startingValues.ElementAt(attributeIndex).Key;

                if (startingValues[attributeKey] >= AttributeMax)
                {
                    continue;
                }

                startingValues[attributeKey]++;
                remainingDice--;
            }

            foreach (var startingValue in startingValues)
            {
                switch (startingValue.Key)
                {
                    case EntityAttributeTypes.Agility:
                        Agility = startingValue.Value;
                        break;
                    case EntityAttributeTypes.Coordination:
                        Coordination = startingValue.Value;
                        break;
                    case EntityAttributeTypes.Physique:
                        Physique = startingValue.Value;
                        break;
                    case EntityAttributeTypes.Intellect:
                        Intellect = startingValue.Value;
                        break;
                    case EntityAttributeTypes.Acumen:
                        Acumen = startingValue.Value;
                        break;
                    case EntityAttributeTypes.Charisma:
                        Charisma = startingValue.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private int GenerateAttributeValue()
        {
            return Random.Range(AttributeMin, AttributeMax + 1);
        }
    }
}
