using System;
using GoRogue.DiceNotation;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Stats
    {
        //todo just temp til we replace the text version of the stats in ui
        private const int PrototypeStatsCap = 99;

        private const int StatCap = 100;
        private const int StatMin = 1;

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
                if (value <= CurrentStatsMin)
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
                if (value < StatMin)
                {
                    _meleeSkill = StatMin;
                }
                else if (value > StatCap)
                {
                    _meleeSkill = StatCap;
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
                if (value < StatMin)
                {
                    _rangedSkill = StatMin;
                }
                else if (value > StatCap)
                {
                    _rangedSkill = StatCap;
                }
                else
                {
                    _rangedSkill = value;
                }
            }
        }
        public int Armor { get; set; } //todo maybe only for some types or can we "equip" natural armor in those cases?
        public int Critical { get; set; } //todo have this ignore damage reduction when implemented
        
        public Stats(Entity parent, Attributes attributes, Skills skills)
        {
            _parent = parent;
            GenerateStats(attributes, skills);
        }

        private void GenerateStats(Attributes attributes, Skills skills)
        {
            MaxHealth =  RollD6(attributes.Physique) + 20;
            CurrentHealth = MaxHealth;

            MaxEnergy = RollD6(skills.Toughness) + 20;
            CurrentEnergy = MaxEnergy;

            MaxMorale = RollD6(attributes.Charisma) + 20;
            CurrentMorale = MaxMorale;

            Initiative = RollD6(attributes.Acumen) + 20;

            Attack = (int) Math.Ceiling(attributes.Physique / 2.0f);

            MeleeSkill = RollD6(attributes.Agility) * 3 + 10;

            RangedSkill = RollD6(attributes.Coordination) * 3 + 10; 

            //Armor = RollD20() + 7;

            //Critical = (int)(attributes.Agility * 2.4 + attributes.Intellect * 2.4 + RollD20()); //todo use wild die for critical success or failures

            MaxActionPoints = 10;
            CurrentActionPoints = MaxActionPoints;
        }

        private void EnforceStatCap(int stat)
        {
            if (stat > PrototypeStatsCap)
            {
                stat = PrototypeStatsCap;
            }
        }

        private int RollD20(int numDice)
        {
            //todo replace this with diceroller
            return Random.Range(1, 21);
        }

        private int RollD12(int numDice)
        {
            //todo replace this with diceroller 
            return Random.Range(1, 13);
        }

        private int RollD10(int numDice)
        {
            //todo replace this with diceroller 
            return Random.Range(1, 11);
        }

        private int RollD6(int numDice)
        {
            //todo replace this with diceroller 

            var total = 0;

            for (var i = 0; i < numDice; i++)
            {
                total += Random.Range(1, 7);
            }

            return total;
        }
    }
}
