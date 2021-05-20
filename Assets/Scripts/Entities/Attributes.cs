using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Attributes
    {
        private const int AttributeMin = 1;
        private const int MagicAttributeMin = 0;
        private const int AttributeMax = 5;

        private const int StartingAttributeDice = 12;

        private readonly Entity _parent;

        //todo need to update Stats when these values change
        public int Agility { get; set; }
        public int Coordination { get; set; }

        private int _physique;
        public int Physique
        {
            get => _physique + GetAllModifiersForStat(EntityAttributeTypes.Physique);
            set 
            {
                if (value > AttributeMax)
                {
                    _physique = AttributeMax;
                }
                else
                {
                    _physique = value;
                }
            }
        }

        private int _intellect;
        public int Intellect
        {
            get => _intellect + GetAllModifiersForStat(EntityAttributeTypes.Intellect);
            set
            {
                if (value > AttributeMax)
                {
                    _intellect = AttributeMax;
                }
                else
                {
                    _intellect = value;
                }
            }
        }

        public int Acumen { get; set; }

        private int _charisma;
        public int Charisma
        {
            get => _charisma + GetAllModifiersForStat(EntityAttributeTypes.Charisma);
            set {
                if (value > AttributeMax)
                {
                    _charisma = AttributeMax;
                }
                else
                {
                    _charisma = value;
                }
            }
        }

        //public int Magic { get; set; } todo not implemented

        public Attributes(Entity parent)
        {
            _parent = parent;
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

        /// <summary>
        /// Returns all modifiers for the given EntityAttributeType.
        /// </summary>
        private int GetAllModifiersForStat(EntityAttributeTypes attribute)
        {
            return GetAdditiveModifiers(attribute) * (1 + GetPercentageModifiers(attribute) / 100);
        }

        /// <summary>
        /// Applies all modifiers to a new value for the given EntityAttributeType.
        /// </summary>
        private int ModifyNewValueForStat(EntityAttributeTypes attribute, int value)
        {
            return GetAdditiveModifiers(attribute) + value * (1 + GetPercentageModifiers(attribute) / 100);
        }

        private int GetAdditiveModifiers(EntityAttributeTypes attribute)
        {
            return (int) GlobalHelper.GetAdditiveModifiers(_parent, attribute);
        }

        private int GetPercentageModifiers(EntityAttributeTypes attribute)
        {
            return (int)GlobalHelper.GetPercentageModifiers(_parent, attribute);
        }
    }
}
