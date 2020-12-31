using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Stats
    {
        //todo just temp til we replace the text version of the stats in ui
        private const int PrototypeStatsCap = 99;

        private const int CurrentStatsMin = 0; 

        private int _maxHealth;
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value > PrototypeStatsCap)
                {
                    _maxHealth = PrototypeStatsCap;
                }
                else
                {
                    _maxHealth = value;
                }
            } }

        private int _currentHealth;
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (value < CurrentStatsMin)
                {
                    _currentHealth = CurrentStatsMin;
                }
                else if(value > MaxHealth)
                {
                    _currentHealth = MaxHealth;
                }
                else
                {
                    _currentHealth = value;
                }
            }
        }

        private int _maxEnergy;
        public int MaxEnergy
        {
            get => _maxEnergy;
            set
            {
                if (value > PrototypeStatsCap)
                {
                    _maxEnergy = PrototypeStatsCap;
                }
                else
                {
                    _maxEnergy = value;
                }
            }
        }

        private int _currentEnergy;
        public int CurrentEnergy
        {
            get => _currentEnergy;
            set
            {
                if (value < CurrentStatsMin)
                {
                    _currentEnergy = CurrentStatsMin;
                }
                else if (value > MaxEnergy)
                {
                    _currentEnergy = MaxEnergy;
                }
                else
                {
                    _currentEnergy = value;
                }
            }
        }

        private int _maxMorale;
        public int MaxMorale
        {
            get => _maxMorale;
            set
            {
                if (value > PrototypeStatsCap)
                {
                    _maxMorale = PrototypeStatsCap;
                }
                else
                {
                    _maxMorale = value;
                }
            }
        }

        private int _currentMorale;
        public int CurrentMorale
        {
            get => _currentMorale;
            set
            {
                if (value < CurrentStatsMin)
                {
                    _currentMorale = CurrentStatsMin;
                }
                else if (value > MaxMorale)
                {
                    _currentMorale = MaxMorale;
                }
                else
                {
                    _currentMorale = value;
                }
            }
        }

        private int _maxActionPoints;
        public int MaxActionPoints
        {
            get => _maxActionPoints;
            set
            {
                if (value > PrototypeStatsCap)
                {
                    _maxActionPoints = PrototypeStatsCap;
                }
                else
                {
                    _maxActionPoints = value;
                }
            }
        }

        private int _currentActionPoints;
        public int CurrentActionPoints
        {
            get => _currentActionPoints;
            set
            {
                if (value < CurrentStatsMin)
                {
                    _currentActionPoints = CurrentStatsMin;
                }
                else if (value > MaxActionPoints)
                {
                    _currentActionPoints = MaxActionPoints;
                }
                else
                {
                    _currentActionPoints = value;
                }
            }
        }

        public int Initiative { get; set; }
        public int Attack { get; set; }
        public int MeleeSkill { get; set; }
        public int RangedSkill { get; set; }
        public int Armor { get; set; }
        public int Critical { get; set; }
        

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
