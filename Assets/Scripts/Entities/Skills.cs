﻿using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Skills
    {
        private const int SkillMin = 1;
        private const int SkillMax = 50;

        private const int StartingSkillDice = 7;
        private const int StartingSkillMax = 5;

        private Entity _parent;

        //todo probably should define setters and getters here
        public int Melee { get; set; }
        public int Ranged { get; set; }
        public int Lockpicking { get; set; }
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

        public Skills(Entity parent)
        {
            _parent = parent;

            GenerateSkillValues();
        }

        //todo this assumes level 1 entity
        //could take in a level (int) to create higher level fellers
        //or even better would be to use whatever level up system we slam together
        public void GenerateSkillValues()
        {
            //todo also need to consider that some classes don't melee or use ranged 
            //could define these values per class 
            var startingValues = new Dictionary<EntitySkillTypes, int>
            {
                { EntitySkillTypes.Melee, SkillMin },
                { EntitySkillTypes.Ranged, SkillMin },
                { EntitySkillTypes.Lockpicking, SkillMin },
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
                    case EntitySkillTypes.Lockpicking:
                        Lockpicking = startingValue.Value;
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
