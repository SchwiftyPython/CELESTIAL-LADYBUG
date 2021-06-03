using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class Party
    {
        private const int MaxSize = 8;
        private const int StartSize = 6;
        private const int MoraleFoodModifier = 10;

        public const int FoodConsumedPerCompanion = 1;

        private Dictionary<string, Entity> _companions;

        public  Entity Derpus { get; set; }

        public int Food { get; set; }
        public int HealthPotions { get; set; }
        public int Gold { get; set; }

        public Party()
        {
            GenerateStartingParty();

            Food = 12;
            HealthPotions = 5;
            Gold = 100;
        }

        //todo refactor
        public void AddCompanion(Entity companion)
        {
            if (_companions == null)
            {
                _companions = new Dictionary<string, Entity>();
            }

            if (_companions.Count >= MaxSize)
            {
                return;
            }

            if (_companions.ContainsKey(companion.Name))
            {
                companion.Name += ", Jr";

                if (_companions.ContainsKey(companion.Name))
                {
                    //todo just generate a different name at this point or something
                    Debug.Log($"{companion.Name} already exists in party!");
                    return;
                }
            }

            _companions.Add(companion.Name, companion);
        }

        public void RemoveCompanion(Entity companion)
        {
            if (_companions == null || !_companions.ContainsKey(companion.Name))
            {
                return;
            }

            _companions.Remove(companion.Name);

            if (_companions.Count <= 0)
            {
                EventMediator eventMediator = Object.FindObjectOfType<EventMediator>();

                eventMediator.Broadcast(GlobalHelper.GameOver, this);
            }
        }

        public Entity GetCompanion(string companionName)
        {
            if (!_companions.ContainsKey(companionName))
            {
                return null;
            }

            return _companions[companionName];
        }

        public List<Entity> GetCompanions()
        {
            return new List<Entity>(_companions.Values);
        }

        public void EatAndHeal()
        {
            var eatResult = Eat();
            var healResult = Heal();

            var totalResult = new List<string>();

            totalResult.AddRange(eatResult);
            totalResult.AddRange(healResult);

            EventMediator eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.PartyEatAndHeal, this, totalResult);
        }

        //todo refactor may not need a list here
        public List<string> Eat()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return new List<string>{"No mouths to feed!"};
            }

            var eatResult = new List<string>();

            //todo choose random order to feed companions. This allows for some to eat and not others when food is low. Then subtract morale for the hungry.

            if (Food < _companions.Count)
            {
                Food = 0;

                foreach (var companion in _companions.Values)
                {
                    companion.SubtractMorale(MoraleFoodModifier);
                }

                eatResult.Add("Not enough food! Party morale drops!");

                Debug.Log("Not enough food! Party morale drops!"); 
            }
            else
            {
                Food -= _companions.Count * FoodConsumedPerCompanion;

                eatResult.Add($"Party ate {_companions.Count * FoodConsumedPerCompanion} food!");

                Debug.Log($"Party ate {_companions.Count * FoodConsumedPerCompanion} food!"); 
            }

            //todo food amount changed event to update party status bar

            return eatResult;
        }

        public void EatForFree()
        {
            if (_companions == null)
            {
                return;
            }

            foreach (var companion in _companions.Values)
            {
                companion.AddMorale(MoraleFoodModifier);
            }
        }

        public List<string> Heal()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return new List<string>{"No one to heal!"};
            }

            if (HealthPotions <= 0)
            {
                Debug.Log("No health potions! Can't heal!");

                return new List<string> { "No health potions! Can't heal!" };
            }

            var healResult = new List<string>();

            //todo choose random order to heal companions.

            foreach (var companion in _companions.Values)
            {
                if (HealthPotions <= 0)
                {
                    return healResult;
                }

                if (companion.Stats.CurrentHealth < companion.Stats.MaxHealth)
                {
                    companion.UseHealthPotion();

                    healResult.Add($"{companion.FirstName()} used a health potion!");

                    HealthPotions--;
                }
            }

            return healResult;
        }

        public void Rest()
        {
            Derpus.Rest();

            if (_companions == null)
            {
                return;
            }

            foreach (var companion in _companions.Values)
            {
                companion.Rest();
            }
        }

        public Entity GetRandomCompanion()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return Derpus;
            }

            var index = Random.Range(0, _companions.Count);

            return _companions.ElementAt(index).Value;
        }

        //todo wrapper method for these that takes in a stat type
        public Entity GetCompanionWithHighestIntellect()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return Derpus;
            }

            Entity smartyPants = null;
            foreach (var companion in _companions.Values)
            {
                if (smartyPants == null || companion.Attributes.Intellect > smartyPants.Attributes.Intellect)
                {
                    smartyPants = companion;
                }
            }

            return smartyPants;
        }

        public Entity GetCompanionWithHighestRangedSkill()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return Derpus;
            }

            Entity bestShot = null;
            foreach (var companion in _companions.Values)
            {
                if (bestShot == null || companion.Stats.RangedSkill > bestShot.Stats.RangedSkill)
                {
                    bestShot = companion;
                }
            }

            return bestShot;
        }

        private void GenerateStartingParty()
        {
            Derpus = new Entity(Race.RaceType.Derpus, EntityClass.Derpus, true);

            _companions = new Dictionary<string, Entity>();

            for (var i = 0; i < StartSize; i++)
            {
                var rType = GlobalHelper.GetRandomEnumValue<Race.RaceType>();

                if (rType == Race.RaceType.Derpus)
                {
                    rType = Race.RaceType.Human;
                }

                var companion = new Spearman(rType, true); //todo change
                
                AddCompanion(companion);
            }
        }
    }
}
