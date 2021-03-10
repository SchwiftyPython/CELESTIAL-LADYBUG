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

        public int Dodge { get; set; }
        public int Lockpicking { get; set; }
        public int Toughness { get; set; }
        public int Healing { get; set; }
        public int Survival { get; set; }
        public int Persuasion { get; set; }

        public Skills()
        {
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
    }
}
