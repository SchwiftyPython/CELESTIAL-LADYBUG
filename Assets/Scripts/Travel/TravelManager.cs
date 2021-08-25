using System;
using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Encounters;
using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Travel
{
    public class TravelManager : MonoBehaviour, ISubscriber
    {
        private const int DemoDaysToDestination = 5;
        private const int FullGameDaysToDestination = 15;

        private int _currentDayOfTravel;

        private MusicController _musicController;
        private TravelMessenger _travelMessenger;

        public int TravelDaysToDestination { get; private set; }

        public Party Party { get; private set; }

        public BiomeType CurrentBiome { get; private set; }

        private void Awake()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            if (!SceneManager.GetActiveScene().name.Equals(GlobalHelper.CombatScene))
            {
                eventMediator.SubscribeToEvent(GlobalHelper.CampingEncounterFinished, this);
            }

            eventMediator.SubscribeToEvent(GlobalHelper.EntityDead, this);

            CurrentBiome = BiomeType.Grassland;

            _currentDayOfTravel = 0;

            TravelDaysToDestination = DemoDaysToDestination;

            _musicController = FindObjectOfType<MusicController>();

            _travelMessenger = FindObjectOfType<TravelMessenger>();
        }

        public void NewParty()
        {
            Party = new Party();
        }

        public void NewInventory()
        {
            var inventory = Inventory.GetPartyInventory();
            inventory.GenerateStartingItems();
        }

        public void StartNewDay()
        {
            var encounterManager = FindObjectOfType<EncounterManager>();
            encounterManager.BuildDecksForNewDay();

            _musicController.PlayTravelMusic();
        }

        public string BuildPartyRewardTextItem(int value, PartySupplyTypes gainType)
        {
            return $"Gained {value} {GlobalHelper.GetEnumDescription(gainType)}!";
        }

        public string BuildPartyLossTextItem(int value, PartySupplyTypes lossType)
        {
            return $"Lost {value} {GlobalHelper.GetEnumDescription(lossType)}!";
        }

        public string BuildCompanionRewardTextItem(Entity companion, int value, EntityStatTypes gainType)
        {
            return $"{companion.Name} gained {value} {GlobalHelper.GetEnumDescription(gainType)}!";
        }

        private string BuildCompanionRewardTextItem(Entity companion, int attributeGainValue, EntityAttributeTypes gainType)
        {
            return $"{companion.Name} gained {attributeGainValue} {GlobalHelper.GetEnumDescription(gainType)}!";
        }

        public string BuildCompanionLossTextItem(Entity companion, int value, EntityStatTypes lossType)
        {
            return $"{companion.Name} lost {value} {GlobalHelper.GetEnumDescription(lossType)}!";
        }

        public void ApplyEncounterReward(Reward reward)
        {
            if (reward.Effects != null && reward.Effects.Count > 0)
            {
                //todo apply effects
            }

            ApplyPartyReward(reward);
            ApplyEntityReward(reward);
        }

        public void ApplyPartyReward(Reward partyReward)
        {
            if (_travelMessenger == null)
            {
                _travelMessenger = FindObjectOfType<TravelMessenger>();
            }

            var rewardsText = new List<TravelMessenger.PartyMessageDto>();

            if (partyReward.PartyGains != null && partyReward.PartyGains.Count > 0)
            {
                foreach (var partyGain in partyReward.PartyGains)
                {
                    var gainType = (PartySupplyTypes)partyGain.Key;

                    switch (gainType)
                    {
                        case PartySupplyTypes.Food:
                            Party.Food += partyGain.Value;
                            break;
                        case PartySupplyTypes.HealthPotions:
                            Party.HealthPotions += partyGain.Value;
                            break;
                        case PartySupplyTypes.Gold:
                            Party.Gold += partyGain.Value;
                            break;
                        default:
                            Debug.Log($"Invalid gain type! {gainType}");
                            break;
                    }

                    var partyDto = new TravelMessenger.PartyMessageDto
                    {
                        Message = BuildPartyRewardTextItem(partyGain.Value, gainType),
                        TextColor = _travelMessenger.rewardColor
                    };

                    rewardsText.Add(partyDto);
                }
            }

            _travelMessenger.QueuePartyMessages(rewardsText);
        }

        public void ApplyEntityReward(Reward entityReward)
        {
            if (entityReward.EntityStatGains != null && entityReward.EntityStatGains.Count > 0)
            {
                foreach (var entityGain in entityReward.EntityStatGains)
                {
                    var targetEntity = entityGain.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanion(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var statGain in entityGain.Value)
                        {
                            var gainType = statGain.Key;

                            var moddedGain = 0;
                            switch (gainType)
                            {
                                case EntityStatTypes.CurrentMorale:
                                    moddedGain = companion.AddMorale(statGain.Value); //modded gain was used to reflect actual gains, but kinda confusing in practice
                                    break;
                                case EntityStatTypes.CurrentHealth:
                                    moddedGain = companion.AddHealth(statGain.Value);
                                    break;
                                case EntityStatTypes.CurrentEnergy:
                                    moddedGain = companion.AddEnergy(statGain.Value);
                                    break;
                                default:
                                    Debug.Log($"Invalid gain type! {gainType}");
                                    break;
                            }

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionRewardTextItem(companion, statGain.Value, gainType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.rewardColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);
                        }
                    }
                }
            }

            if (entityReward.EntityAttributeGains != null && entityReward.EntityAttributeGains.Count > 0)
            {
                foreach (var entityGain in entityReward.EntityAttributeGains)
                {
                    var targetEntity = entityGain.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanion(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var attributeGain in entityGain.Value)
                        {
                            var gainType = attributeGain.Key;

                            switch (gainType)
                            {
                                case EntityAttributeTypes.Agility:
                                    companion.Attributes.Agility += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Coordination:
                                    companion.Attributes.Coordination += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Physique:
                                    companion.Attributes.Physique += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Intellect:
                                    companion.Attributes.Intellect += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Acumen:
                                    companion.Attributes.Acumen += attributeGain.Value;
                                    break;
                                case EntityAttributeTypes.Charisma:
                                    companion.Attributes.Charisma += attributeGain.Value;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionRewardTextItem(companion, attributeGain.Value, gainType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.rewardColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);
                        }
                    }
                }
            }
        }

        public void ApplyEncounterPenalty(Penalty penalty)
        {
            if (penalty.Effects != null && penalty.Effects.Count > 0)
            {
                //todo apply effects
            }

            ApplyPartyPenalty(penalty);
            ApplyEntityPenalty(penalty);
        }

        private void ApplyPartyPenalty(Penalty partyPenalty)
        {
            if (_travelMessenger == null)
            {
                _travelMessenger = FindObjectOfType<TravelMessenger>();
            }

            var penaltiesText = new List<TravelMessenger.PartyMessageDto>();

            if (partyPenalty.PartyLosses != null && partyPenalty.PartyLosses.Count > 0)
            {
                foreach (var partyLoss in partyPenalty.PartyLosses)
                {
                    var lossType = partyLoss.Key;

                    switch (lossType)
                    {
                        case PartySupplyTypes.Food:
                            Party.Food -= partyLoss.Value;
                            break;
                        case PartySupplyTypes.HealthPotions:
                            Party.HealthPotions -= partyLoss.Value;
                            break;
                        case PartySupplyTypes.Gold:
                            Party.Gold -= partyLoss.Value;
                            break;
                        default:
                            Debug.Log($"Invalid loss type! {lossType}");
                            break;
                    }

                    var partyDto = new TravelMessenger.PartyMessageDto
                    {
                        Message = BuildPartyLossTextItem(partyLoss.Value, lossType),
                        TextColor = _travelMessenger.penaltyColor
                    };

                    penaltiesText.Add(partyDto);
                }
            }

            _travelMessenger.QueuePartyMessages(penaltiesText);
        }

        private void ApplyEntityPenalty(Penalty entityPenalty)
        {
            if (entityPenalty.EntityLosses != null && entityPenalty.EntityLosses.Count > 0)
            {
                foreach (var entityLoss in entityPenalty.EntityLosses)
                {
                    var targetEntity = entityLoss.Key;

                    Entity companion;

                    if (targetEntity.IsDerpus())
                    {
                        companion = Party.Derpus;
                    }
                    else
                    {
                        //it's possible the entity isn't in the party anymore so this is how we check off the top of my head
                        companion = Party.GetCompanion(targetEntity.Name);
                    }

                    if (companion != null)
                    {
                        foreach (var statLoss in entityLoss.Value)
                        {
                            var lossType = statLoss.Key;

                            var entityDto = new TravelMessenger.EntityMessageDto
                            {
                                Message = BuildCompanionLossTextItem(companion, statLoss.Value, lossType),
                                Portrait = companion.Portrait,
                                TextColor = _travelMessenger.penaltyColor
                            };

                            _travelMessenger.QueueEntityMessage(entityDto);

                            switch (lossType)
                            {
                                case EntityStatTypes.CurrentMorale:
                                    companion.SubtractMorale(statLoss.Value);
                                    break;
                                case EntityStatTypes.CurrentHealth:
                                    companion.SubtractHealth(statLoss.Value);
                                    break;
                                case EntityStatTypes.CurrentEnergy:
                                    companion.SubtractEnergy(statLoss.Value);
                                    break;
                                default:
                                    Debug.Log($"Invalid loss type! {lossType}");
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator?.UnsubscribeFromAllEvents(this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.CampingEncounterFinished))
            {
                //todo need standard energy gain for camping events
                //some bool that indicates standard energy gain
                //if false, energy gain or loss handled elsewhere

                Party.EatAndHeal();

                _currentDayOfTravel++;

                var countsAsDayTraveled = parameter != null && (bool) parameter;

                if (countsAsDayTraveled)
                {
                    TravelDaysToDestination--;
                }

                if (TravelDaysToDestination <= 0)
                {
                    var eventMediator = FindObjectOfType<EventMediator>();
                    eventMediator.Broadcast(GlobalHelper.YouWon, this);
                }
                else
                {
                    StartNewDay();
                }
            }
            else if (eventName.Equals(GlobalHelper.EntityDead))
            {
                if (!(broadcaster is Entity deadGuy) || !deadGuy.IsPlayer())
                {
                    return;
                }

                Party.RemoveCompanion(deadGuy);

                var entityDto = new TravelMessenger.EntityMessageDto
                {
                    Message = $"{deadGuy.Name} died!",
                    TextColor = _travelMessenger.penaltyColor,
                    Portrait = deadGuy.Portrait
                };

                _travelMessenger.QueueEntityMessage(entityDto);
            }
        }
    }
}
