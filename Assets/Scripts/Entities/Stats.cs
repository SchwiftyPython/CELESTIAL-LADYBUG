using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Stats
    {
        //todo just temp til we replace the text version of the stats in ui
        private const int PrototypeStatsCap = 99;

        private const int SkillCap = 100;
        private const int SkillMin = 11;

        private const int CurrentStatsMin = 0;

        private Entity _parent;

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
                    EventMediator.Instance.Broadcast(GlobalHelper.EntityDead, _parent);
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

                    if (_parent.IsDerpus())
                    {
                        EventMediator.Instance.Broadcast(GlobalHelper.DerpusNoEnergy, this);
                    }
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
                    EventMediator.Instance.Broadcast(GlobalHelper.MentalBreak, _parent);
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

        private int _meleeSkill;
        public int MeleeSkill
        {
            get => _meleeSkill;
            set
            {
                if (value < SkillMin)
                {
                    _meleeSkill = SkillMin;
                }
                else if (value > SkillCap)
                {
                    _meleeSkill = SkillCap;
                }
                else
                {
                    _meleeSkill = value;
                }
            }
        }

        private int _rangedSkill;
        public int RangedSkill
        {
            get => _rangedSkill;
            set 
            {
                if (value < SkillMin)
                {
                    _rangedSkill = SkillMin;
                }
                else if (value > SkillCap)
                {
                    _rangedSkill = SkillCap;
                }
                else
                {
                    _rangedSkill = value;
                }
            }
        }
        public int Armor { get; set; } //todo maybe only for some types or can we "equip" natural armor in those cases?
        public int Critical { get; set; } //todo have this ignore damage reduction when implemented
        
        public Stats(Entity parent, Attributes attributes)
        {
            _parent = parent;
            GenerateStats(attributes);
        }

        private void GenerateStats(Attributes attributes)
        {
            //todo these are arbitrary numbers - might need to tweak

            MaxHealth = (int) (attributes.Might * 3.1f + RollD20());
            CurrentHealth = MaxHealth;

            MaxEnergy = (int) (attributes.Might * 3.6f + RollD20());
            CurrentEnergy = MaxEnergy;

            MaxMorale = (int) (RollD20() * 2.3f) + 30;
            CurrentMorale = MaxMorale;

            Initiative = (int) (attributes.Speed * 2.1 + attributes.Intellect * 2.1 + RollD20());

            Attack = (int) (attributes.Might * 4.3);

            MeleeSkill = RollD10() * 10 + RollD10();

            RangedSkill = RollD10() * 10 + RollD10();

            //Armor = RollD20() + 7;

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

        private int RollD12()
        {
            //todo replace this with diceroller 
            return Random.Range(1, 13);
        }

        private int RollD10()
        {
            //todo replace this with diceroller 
            return Random.Range(1, 11);
        }
    }
}
