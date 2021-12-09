using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Skills
    {
        private const int SkillMin = 1;
        private const int SkillMax = 50;

        private const int StartingSkillDice = 7;
        private const int StartingSkillMax = 5;

        private readonly Dictionary<EntityClass, Dictionary<EntitySkillTypes, int>> _classStartingVals =
            new Dictionary<EntityClass, Dictionary<EntitySkillTypes, int>>
            {
                {
                        EntityClass.Spearman, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 4 },
                            { EntitySkillTypes.Ranged, 1 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    },
                    {
                        EntityClass.Crossbowman, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 1 },
                            { EntitySkillTypes.Ranged, 4 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    },
                    {
                        EntityClass.ManAtArms, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 4 },
                            { EntitySkillTypes.Ranged, 1 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    },
                    {
                        EntityClass.Gladiator, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 4 },
                            { EntitySkillTypes.Ranged, 1 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    },
                    {
                        EntityClass.Wizard, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 1 },
                            { EntitySkillTypes.Ranged, 4 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 2 },
                            { EntitySkillTypes.Healing, 3 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 3 }
                        }
                    },
                    {
                        EntityClass.BattleMage, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 3 },
                            { EntitySkillTypes.Ranged, 3 },
                            { EntitySkillTypes.Sneak, 2 },
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 3 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    },
                    {
                        EntityClass.Knight, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 4 },
                            { EntitySkillTypes.Ranged, 1 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 4 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 3 }
                        }
                    },
                    {
                        EntityClass.Paladin, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 4 },
                            { EntitySkillTypes.Ranged, 1 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 4 }
                        }
                    },
                    {
                        EntityClass.Derpus, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 4 },
                            { EntitySkillTypes.Ranged, 1 },
                            { EntitySkillTypes.Sneak,  2},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 2 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    },
                    {
                        EntityClass.Beast, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 3 },
                            { EntitySkillTypes.Ranged, 3 },
                            { EntitySkillTypes.Sneak,  3},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 3 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    },
                    {
                        EntityClass.Ethereal, new Dictionary<EntitySkillTypes, int>
                        {
                            { EntitySkillTypes.Melee, 3 },
                            { EntitySkillTypes.Ranged, 3 },
                            { EntitySkillTypes.Sneak,  3},
                            { EntitySkillTypes.Endurance, 3 },
                            { EntitySkillTypes.Healing, 2 },
                            { EntitySkillTypes.Survival, 3 },
                            { EntitySkillTypes.Persuasion, 2 }
                        }
                    }
            };

        [ES3NonSerializable] private Entity _parent;

        //todo probably should define setters and getters here
        public int Melee { get; set; }
        public int Ranged { get; set; }
        public int Sneak { get; set; }
        public int Endurance { get; set; }
        public int Healing { get; set; }
        public int Survival { get; set; }

        private int _persuasion;
        public int Persuasion 
        {
            get
            {
                var moddedPers = _persuasion + GetAllModifiersForStat(EntitySkillTypes.Persuasion);

                if (moddedPers > SkillMax)
                {
                    return SkillMax;
                }

                return _persuasion;
            }
            set
            {
                if (value < SkillMin)
                {
                    _persuasion = SkillMin;
                }
                else if (value > SkillMax)
                {
                    _persuasion = SkillMax;
                }
                else
                {
                    _persuasion = value;
                }
            }
        }

        public Skills()
        {
        }

        public Skills(Entity parent)
        {
            SetParent(parent);

            GenerateSkillValues(parent.EntityClass);
        }

        public void SetParent(Entity parent)
        {
            _parent = parent;
        }

        private void GenerateSkillValues(EntityClass eClass)
        {
            if (!_classStartingVals.ContainsKey(eClass))
            {
                Debug.LogError($"{eClass} not found in class starting values!");
                return;
            }

            var startingVals = _classStartingVals[eClass];

            var mods = new List<int> { -1, 0, 1 };

            foreach (var startingValue in startingVals)
            {
                var mod = mods[Random.Range(0, mods.Count)];

                switch (startingValue.Key)
                {
                    case EntitySkillTypes.Melee:
                        Melee = startingValue.Value + mod;
                        break;
                    case EntitySkillTypes.Ranged:
                        Ranged = startingValue.Value + mod;
                        break;
                    case EntitySkillTypes.Sneak:
                        Sneak = startingValue.Value + mod;
                        break;
                    case EntitySkillTypes.Endurance:
                        Endurance = startingValue.Value + mod;
                        break;
                    case EntitySkillTypes.Healing:
                        Healing = startingValue.Value + mod;
                        break;
                    case EntitySkillTypes.Survival:
                        Survival = startingValue.Value + mod;
                        break;
                    case EntitySkillTypes.Persuasion:
                        Persuasion = startingValue.Value + mod;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void GenerateSkillValues()
        {
            //todo also need to consider that some classes don't melee or use ranged 
            //could define these values per class 
            var startingValues = new Dictionary<EntitySkillTypes, int>
            {
                { EntitySkillTypes.Melee, SkillMin },
                { EntitySkillTypes.Ranged, SkillMin },
                { EntitySkillTypes.Sneak, SkillMin },
                { EntitySkillTypes.Endurance, SkillMin },
                { EntitySkillTypes.Healing, SkillMin },
                { EntitySkillTypes.Survival, SkillMin },
                { EntitySkillTypes.Persuasion, SkillMin }
            };

            var remainingDice = StartingSkillDice;

            while (remainingDice > 0)
            {
                var attributeIndex = Random.Range(0, startingValues.Count);

                var attributeKey = startingValues.ElementAt(attributeIndex).Key;

                if (startingValues[attributeKey] >= StartingSkillMax)
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
                    case EntitySkillTypes.Melee:
                        Melee = startingValue.Value;
                        break;
                    case EntitySkillTypes.Ranged:
                        Ranged = startingValue.Value;
                        break;
                    case EntitySkillTypes.Sneak:
                        Sneak = startingValue.Value;
                        break;
                    case EntitySkillTypes.Endurance:
                        Endurance = startingValue.Value;
                        break;
                    case EntitySkillTypes.Healing:
                        Healing = startingValue.Value;
                        break;
                    case EntitySkillTypes.Survival:
                        Survival = startingValue.Value;
                        break;
                    case EntitySkillTypes.Persuasion:
                        Persuasion = startingValue.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns all modifiers for the given StatType.
        /// </summary>
        private int GetAllModifiersForStat(EntitySkillTypes stat)
        {
            return (int)(GetAdditiveModifiers(stat) * (1 + GetPercentageModifiers(stat) / 100));
        }

        /// <summary>
        /// Applies all modifiers to a new value for the given StatType.
        /// </summary>
        private int ModifyNewValueForStat(EntitySkillTypes stat, int value)
        {
            return (int)(GetAdditiveModifiers(stat) + value * (1 + GetPercentageModifiers(stat) / 100));
        }

        /// <summary>
        /// Returns all additive modifiers in equipment and abilities for the given StatType.
        /// </summary>
        private float GetAdditiveModifiers(EntitySkillTypes stat)
        {
            float total = 0;

            var equipment = _parent.GetEquipment();

            if (equipment == null)
            {
                return total;
            }

            foreach (EquipLocation slot in Enum.GetValues(typeof(EquipLocation)))
            {
                var item = equipment.GetItemInSlot(slot);

                if (item == null)
                {
                    continue;
                }

                total += item.GetAdditiveModifiers(stat);
            }

            var abilities = _parent.Abilities;

            foreach (var ability in abilities.Values)
            {
                if (!(ability is IModifierProvider provider))
                {
                    continue;
                }

                total += provider.GetAdditiveModifiers(stat);
            }

            return total;
        }

        /// <summary>
        /// Returns all percentage modifiers in equipment and abilities for the given StatType.
        /// </summary>
        private float GetPercentageModifiers(EntitySkillTypes stat)
        {
            float total = 0;

            var equipment = _parent.GetEquipment();

            if (equipment == null)
            {
                return total;
            }

            foreach (EquipLocation slot in Enum.GetValues(typeof(EquipLocation)))
            {
                var item = equipment.GetItemInSlot(slot);

                if (item == null)
                {
                    continue;
                }

                total += item.GetPercentageModifiers(stat);
            }

            var abilities = _parent.Abilities;

            foreach (var ability in abilities.Values)
            {
                if (!(ability is IModifierProvider provider))
                {
                    continue;
                }

                total += provider.GetPercentageModifiers(stat);
            }

            return total;
        }
    }
}
