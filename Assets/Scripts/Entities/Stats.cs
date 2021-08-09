using System;
using Assets.Scripts.Abilities;
using Object = UnityEngine.Object;
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

        private readonly Entity _parent;

        private int _maxHealth;

        public int MaxHealth
        {
            get => _maxHealth + GetAllModifiersForStat(EntityStatTypes.MaxHealth);
            private set
            {
                if (value > PrototypeStatsCap)
                {
                    _maxHealth = PrototypeStatsCap;
                }
                else
                {
                    _maxHealth = value;
                }
            }
        }

        private int _currentHealth;
        public int CurrentHealth
        {
            get
            {
                if (_currentHealth > MaxHealth)
                {
                    _currentHealth = MaxHealth;
                }

                return _currentHealth;
            }
            set
            {
                if (value > _currentHealth)
                {
                    var moddedValue = ModifyNewValueForStat(EntityStatTypes.CurrentHealth, value - _currentHealth);

                    value = _currentHealth + moddedValue;
                }

                if (value <= CurrentStatsMin)
                {
                    //todo need to encapsulate this if we add another will not die ability
                    if (GameManager.Instance.InCombat() && _parent.HasAbility(typeof(EndangeredEndurance)) &&
                        !((EndangeredEndurance) _parent.Abilities[typeof(EndangeredEndurance)])
                            .SavedFromDeathThisBattle())
                    {
                        _parent.Abilities[typeof(EndangeredEndurance)].Use();
                    }
                    else
                    {
                        _currentHealth = CurrentStatsMin;

                        foreach (var ability in _parent.Abilities.Values) //todo might make a Die method lol
                        {
                            ability.Terminate();
                        }

                        var eAudio = _parent.CombatSpriteInstance.GetComponent<EntityAudio>();

                        eAudio.Die(_parent.DieSound);

                        var eventMediator = Object.FindObjectOfType<EventMediator>();
                        eventMediator.Broadcast(GlobalHelper.EntityDead, _parent);
                    }
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
                        var eventMediator = Object.FindObjectOfType<EventMediator>();
                        eventMediator.Broadcast(GlobalHelper.DerpusNoEnergy, this);
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

                    var eventMediator = Object.FindObjectOfType<EventMediator>();
                    eventMediator.Broadcast(GlobalHelper.MentalBreak, _parent);
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
            get => _maxActionPoints + GetAllModifiersForStat(EntityStatTypes.MaxActionPoints);
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
            get
            {
                if (_currentActionPoints > MaxActionPoints)
                {
                    _currentActionPoints = MaxActionPoints;
                }

                return _currentActionPoints;
            }
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

            MaxEnergy = RollD6(skills.Endurance) + 20;
            CurrentEnergy = MaxEnergy;

            MaxMorale = RollD6(attributes.Charisma) + 20;
            CurrentMorale = MaxMorale;

            Initiative = RollD6(attributes.Acumen) + 20;

            MaxActionPoints = 10;
            CurrentActionPoints = CurrentActionPoints;
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

        /// <summary>
        /// Returns all modifiers for the given StatType.
        /// </summary>
        private int GetAllModifiersForStat(EntityStatTypes stat)
        {
            return (int)(GetAdditiveModifiers(stat) * (1 + GetPercentageModifiers(stat) / 100));
        }

        /// <summary>
        /// Applies all modifiers to a new value for the given StatType.
        /// </summary>
        private int ModifyNewValueForStat(EntityStatTypes stat, int value)
        {
            return (int) (GetAdditiveModifiers(stat) + value * (1 + GetPercentageModifiers(stat) / 100));
        }

        /// <summary>
        /// Returns all additive modifiers in equipment and abilities for the given StatType.
        /// </summary>
        private float GetAdditiveModifiers(EntityStatTypes stat)
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

            if (stat == EntityStatTypes.MaxActionPoints)
            {
                total += GetMaxActionPointsModifier();
            }

            return total;
        }

        /// <summary>
        /// Returns all percentage modifiers in equipment and abilities for the given StatType.
        /// </summary>
        private float GetPercentageModifiers(EntityStatTypes stat)
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

        private int GetMaxActionPointsModifier()
        {
            var energyPercentage = ((float)CurrentEnergy / MaxEnergy) * 100;

            if (energyPercentage < 25)
            {
                return _maxActionPoints - 2;
            }

            if (energyPercentage < 50)
            {
                return _maxActionPoints - 1;
            }

            return 0;
        }
    }
}
