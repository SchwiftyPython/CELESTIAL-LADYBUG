using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Attributes
    {
        private const int AttributeMin = 1;
        private const int MagicAttributeMin = 0;
        private const int AttributeMax = 5;

        private const int StartingAttributeDice = 12;

        private readonly Dictionary<EntityClass, Dictionary<EntityAttributeTypes, int>> _classStartingVals =
            new Dictionary<EntityClass, Dictionary<EntityAttributeTypes, int>>
            {
                {
                        EntityClass.Spearman, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 4 },
                            { EntityAttributeTypes.Coordination, 3 },
                            { EntityAttributeTypes.Physique, 4 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 2 }
                        }
                    },
                    {
                        EntityClass.Crossbowman, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 3 },
                            { EntityAttributeTypes.Coordination, 3 },
                            { EntityAttributeTypes.Physique, 3 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 2 },
                            { EntityAttributeTypes.Charisma, 3 }
                        }
                    },
                    {
                        EntityClass.ManAtArms, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 4 },
                            { EntityAttributeTypes.Coordination, 3 },
                            { EntityAttributeTypes.Physique, 4 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 2 }
                        }
                    },
                    {
                        EntityClass.Gladiator, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 3 },
                            { EntityAttributeTypes.Coordination, 3 },
                            { EntityAttributeTypes.Physique, 3 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 2 }
                        }
                    },
                    {
                        EntityClass.Wizard, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 2 },
                            { EntityAttributeTypes.Coordination, 2 },
                            { EntityAttributeTypes.Physique, 2 },
                            { EntityAttributeTypes.Intellect, 3 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 2 }
                        }
                    },
                    {
                        EntityClass.BattleMage, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 2 },
                            { EntityAttributeTypes.Coordination, 2 },
                            { EntityAttributeTypes.Physique, 2 },
                            { EntityAttributeTypes.Intellect, 3 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 3 }
                        }
                    },
                    {
                        EntityClass.Knight, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 4 },
                            { EntityAttributeTypes.Coordination, 3 },
                            { EntityAttributeTypes.Physique, 4 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 2 }
                        }
                    },
                    {
                        EntityClass.Paladin, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 2 },
                            { EntityAttributeTypes.Coordination, 2 },
                            { EntityAttributeTypes.Physique, 2 },
                            { EntityAttributeTypes.Intellect, 3 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 3 }
                        }
                    },
                    {
                        EntityClass.Derpus, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 2 },
                            { EntityAttributeTypes.Coordination, 2 },
                            { EntityAttributeTypes.Physique, 4 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 2 },
                            { EntityAttributeTypes.Charisma, 2 }
                        }
                    },
                    {
                        EntityClass.Beast, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 3 },
                            { EntityAttributeTypes.Coordination, 3 },
                            { EntityAttributeTypes.Physique, 3 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 3 },
                            { EntityAttributeTypes.Charisma, 2 }
                        }
                    },
                    {
                        EntityClass.Ethereal, new Dictionary<EntityAttributeTypes, int>
                        {
                            { EntityAttributeTypes.Agility, 3 },
                            { EntityAttributeTypes.Coordination, 3 },
                            { EntityAttributeTypes.Physique, 3 },
                            { EntityAttributeTypes.Intellect, 2 },
                            { EntityAttributeTypes.Acumen, 2 },
                            { EntityAttributeTypes.Charisma, 3 }
                        }
                    }
            };

        [ES3NonSerializable] private Entity _parent;

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
                    _parent.Stats.UpdateMaxHealth(this);
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

        private int _acumen;

        public int Acumen
        {
            get => _acumen + GetAllModifiersForStat(EntityAttributeTypes.Acumen);
            set
            {
                if (value > AttributeMax)
                {
                    _acumen = AttributeMax;
                }
                else
                {
                    _acumen = value;
                    _parent.Stats.UpdateInitiative(this);
                }
            }
        }

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
                    _parent.Stats.UpdateMaxMorale(this);
                }
            }
        }

        //public int Magic { get; set; } todo not implemented

        public Attributes()
        {
        }

        public Attributes(Entity parent)
        {
            SetParent(parent);
            GenerateAttributeValues(parent.EntityClass);
        }

        public void SetParent(Entity parent)
        {
            _parent = parent;
        }

        private void GenerateAttributeValues(EntityClass eClass)
        {
            if (!_classStartingVals.ContainsKey(eClass))
            {
                Debug.LogError($"{eClass} not found in class starting values!");
                return;
            }

            var startingVals = _classStartingVals[eClass];

            var mods = new List<int>{ -1, 0, 1 };

            foreach (var startingValue in startingVals)
            {
                var mod = mods[Random.Range(0, mods.Count)];

                switch (startingValue.Key)
                {
                    case EntityAttributeTypes.Agility:
                        Agility = startingValue.Value + mod;
                        break;
                    case EntityAttributeTypes.Coordination:
                        Coordination = startingValue.Value + mod;
                        break;
                    case EntityAttributeTypes.Physique:
                        Physique = startingValue.Value + mod;
                        break;
                    case EntityAttributeTypes.Intellect:
                        Intellect = startingValue.Value + mod;
                        break;
                    case EntityAttributeTypes.Acumen:
                        Acumen = startingValue.Value + mod;
                        break;
                    case EntityAttributeTypes.Charisma:
                        Charisma = startingValue.Value + mod;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
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
