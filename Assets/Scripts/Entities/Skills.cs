using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Skills
    {
        private const int SkillMin = 0;
        private const int SkillMax = 5;

        private const int StartingSkillDice = 7;

        private Entity _parent;

        private int _dodge;
        public int Dodge 
        {
            get
            {
                var moddedDodge = _dodge + GetAllModifiersForStat(EntitySkillTypes.Dodge);

                if (moddedDodge > SkillMax)
                {
                    return SkillMax;
                }

                return moddedDodge;
            }
            private set
            {
                if (value < SkillMin)
                {
                    _dodge = SkillMin;
                }
                else if (value > SkillMax)
                {
                    _dodge = SkillMax;
                }
                else
                {
                    _dodge = value;
                }
            }
        }
        public int Lockpicking { get; set; }
        public int Toughness { get; set; }
        public int Healing { get; set; }
        public int Survival { get; set; }
        public int Persuasion { get; set; }

        public Skills(Entity parent)
        {
            _parent = parent;

            GenerateSkillValues();
        }

        public void GenerateSkillValues()
        {
            var startingValues = new Dictionary<EntitySkillTypes, int>
            {
                { EntitySkillTypes.Dodge, SkillMin },
                { EntitySkillTypes.Lockpicking, SkillMin },
                { EntitySkillTypes.Toughness, SkillMin },
                { EntitySkillTypes.Healing, SkillMin },
                { EntitySkillTypes.Survival, SkillMin },
                { EntitySkillTypes.Persuasion, SkillMin }
            };

            var remainingDice = StartingSkillDice;

            while (remainingDice > 0)
            {
                var attributeIndex = Random.Range(0, startingValues.Count);

                var attributeKey = startingValues.ElementAt(attributeIndex).Key;

                if (startingValues[attributeKey] >= SkillMax)
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
                    case EntitySkillTypes.Dodge:
                        Dodge = startingValue.Value;
                        break;
                    case EntitySkillTypes.Lockpicking:
                        Lockpicking = startingValue.Value;
                        break;
                    case EntitySkillTypes.Toughness:
                        Toughness = startingValue.Value;
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

                foreach (var modifier in item.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            var abilities = _parent.Abilities;

            foreach (var ability in abilities.Values)
            {
                if (!(ability is IModifierProvider provider))
                {
                    continue;
                }

                foreach (var modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
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

                foreach (var modifier in item.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            var abilities = _parent.Abilities;

            foreach (var ability in abilities.Values)
            {
                if (!(ability is IModifierProvider provider))
                {
                    continue;
                }

                foreach (var modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
    }
}
