using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Companions;
using Assets.Scripts.UI;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Party
    {
        private const int MaxSize = 6;
        private const int StartSize = 4;
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
        }

        public bool PartyDead()
        {
            return _companions.Count <= 0;
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

            var travelMessenger = Object.FindObjectOfType<TravelMessenger>();

            travelMessenger.QueuePartyMessages(eatResult);

            foreach (var message in healResult)
            {
                travelMessenger.QueueEntityMessage(message);
            }
        }

        public List<TravelMessenger.PartyMessageDto> Eat()
        {
            var travelMessenger = Object.FindObjectOfType<TravelMessenger>();

            if (_companions == null || _companions.Count < 1)
            {
                var partyDto = new TravelMessenger.PartyMessageDto
                {
                    Message = "No mouths to feed!",
                    TextColor = new Color32(1, 1, 1, 1)
                };

                return new List<TravelMessenger.PartyMessageDto> {partyDto};
            }

            var eatResult = new List<TravelMessenger.PartyMessageDto>();

            //todo choose random order to feed companions. This allows for some to eat and not others when food is low. Then subtract morale for the hungry.

            if (Food < _companions.Count)
            {
                Food = 0;

                foreach (var companion in _companions.Values)
                {
                    companion.SubtractMorale(MoraleFoodModifier);
                }

                var partyDto = new TravelMessenger.PartyMessageDto
                {
                    Message = "Not enough food! Party morale drops!",
                    TextColor = travelMessenger.penaltyColor
                };

                eatResult.Add(partyDto);

                Debug.Log("Not enough food! Party morale drops!"); 
            }
            else
            {
                Food -= _companions.Count * FoodConsumedPerCompanion;

                var partyDto = new TravelMessenger.PartyMessageDto
                {
                    Message = $"Party ate {_companions.Count * FoodConsumedPerCompanion} food!",
                    TextColor = travelMessenger.rewardColor
                };

                eatResult.Add(partyDto);

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

        public List<TravelMessenger.EntityMessageDto> Heal()
        {
            var travelMessenger = Object.FindObjectOfType<TravelMessenger>();

            if (HealthPotions <= 0)
            {
                Debug.Log("No health potions! Can't heal!");

                var partyDto = new TravelMessenger.EntityMessageDto
                {
                    Message = "No health potions! Can't heal!",
                    TextColor = travelMessenger.penaltyColor,
                    Portrait = null
                };

                return new List<TravelMessenger.EntityMessageDto> { partyDto };
            }

            var healResult = new List<TravelMessenger.EntityMessageDto>();

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

                    var partyDto = new TravelMessenger.EntityMessageDto
                    {
                        Message = $"{companion.FirstName()} used a health potion!",
                        TextColor = travelMessenger.rewardColor,
                        Portrait = companion.Portrait
                    };

                    healResult.Add(partyDto);

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

        public List<Entity> GetRandomCompanions(int numCompanions)
        {
            if (_companions == null || _companions.Count < 1)
            {
                return new List<Entity> { Derpus };
            }

            var remaining = new List<Entity>(_companions.Values);

            var picked = new List<Entity>();

            while (remaining.Count > 0 && picked.Count < numCompanions)
            {
                var index = Random.Range(0, remaining.Count);

                var companion = remaining[index];

                picked.Add(companion);

                remaining.Remove(companion);
            }

            return picked;
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
                if (bestShot == null || companion.Skills.Ranged > bestShot.Skills.Ranged)
                {
                    bestShot = companion;
                }
            }

            return bestShot;
        }

        public Entity GetCompanionWithHighestCoordination()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return Derpus;
            }

            Entity bestCoord = null;
            foreach (var companion in _companions.Values)
            {
                if (bestCoord == null || companion.Attributes.Coordination > bestCoord.Attributes.Coordination)
                {
                    bestCoord = companion;
                }
            }

            return bestCoord;
        }

        public Entity GetCompanionWithHighestHealing()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return Derpus;
            }

            Entity bestHealing = null;
            foreach (var companion in _companions.Values)
            {
                if (bestHealing == null || companion.Skills.Healing > bestHealing.Skills.Healing)
                {
                    bestHealing = companion;
                }
            }

            return bestHealing;
        }

        public Entity GetCompanionWithLowestEndurance()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return Derpus;
            }

            Entity worstEndurance = null;
            foreach (var companion in _companions.Values)
            {
                if (worstEndurance == null || companion.Skills.Endurance < worstEndurance.Skills.Endurance)
                {
                    worstEndurance = companion;
                }
            }

            return worstEndurance;
        }

        public int GetTotalPartyEndurance()
        {
            return _companions.Sum(companion => companion.Value.Skills.Endurance);
        }

        public int GetTotalPartyPhysique()
        {
            return _companions.Sum(companion => companion.Value.Attributes.Physique);
        }

        public bool IsFull()
        {
            return _companions.Count >= MaxSize;
        }

        private void SetAllMoraleToOne()
        {
            Derpus.Stats.CurrentMorale = 1;

            foreach (var companion in _companions.Values)
            {
                companion.Stats.CurrentMorale = 1;
            }
        }

        private void SetAllHealthToOne()
        {
            foreach (var companion in _companions.Values)
            {
                companion.Stats.CurrentHealth = 1;
            }
        }

        private void GenerateStartingParty()
        {
            Derpus = new Entity(Race.RaceType.Derpus, EntityClass.Derpus, true);

            _companions = new Dictionary<string, Entity>();

            var entityStore = Object.FindObjectOfType<EntityPrefabStore>();

            for (var i = 0; i < StartSize; i++)
            {
                var companion = entityStore.GetRandomCompanion();

                AddCompanion(companion);
            }
        }
    }
}
