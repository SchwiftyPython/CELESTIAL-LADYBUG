using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Stats
    {
        //todo just temp til we replace the text version of the stats in ui
        private const int PrototypeStatsCap = 99;

        private const int CurrentStatsMin = 0; //todo custom setters for all stats to enforce mins and maxes

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxEnergy { get; set; }
        public int CurrentEnergy { get; set; }
        public int MaxMorale { get; set; }
        public int CurrentMorale { get; set; }
        public int Initiative { get; set; }
        public int Attack { get; set; }
        public int MeleeSkill { get; set; }
        public int RangedSkill { get; set; }
        public int Armor { get; set; }
        public int Critical { get; set; }
        public int MaxActionPoints { get; set; }
        public int CurrentActionPoints { get; set; }

        public Stats(Attributes attributes)
        {
            GenerateStats(attributes);
        }

        private void GenerateStats(Attributes attributes)
        {
            //todo these are arbitrary numbers - need to tweak

            MaxHealth = (int) (attributes.Might * 3.1f + RollD20());
            EnforceStatCap(MaxHealth);
            CurrentHealth = MaxHealth;

            MaxEnergy = (int) (attributes.Might * 3.6f + RollD20());
            EnforceStatCap(MaxEnergy);
            CurrentEnergy = MaxEnergy;

            MaxMorale = (int) (RollD20() * 2.3f) + 30;
            EnforceStatCap(MaxMorale);
            CurrentMorale = MaxMorale;

            Initiative = (int) (attributes.Speed * 2.1 + attributes.Intellect * 2.1 + RollD20());

            Attack = (int) (attributes.Might * 4.3);

            MeleeSkill = (int) (RollD20() * 4.4f) + 5;

            RangedSkill = (int) (RollD20() * 4.4f) + 5;

            Armor = RollD20() + 7;

            Critical = (int)(attributes.Speed * 2.4 + attributes.Intellect * 2.4 + RollD20());

            MaxActionPoints = 10 - RollD20() / 5;
            CurrentActionPoints = MaxActionPoints;
        }

        private void EnforceStatCap(int stat)
        {
            if (stat > PrototypeStatsCap)
            {
                stat = PrototypeStatsCap;
            }
        }

        private int RollD20()
        {
            //todo replace this with diceroller 
            return Random.Range(1, 21);
        }
    }
}
