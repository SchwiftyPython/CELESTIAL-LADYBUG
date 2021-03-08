﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Encounters;
using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Travel
{
    public class TravelManager : MonoBehaviour, ISubscriber
    {
        private const int DemoDaysToDestination = 5;
        private const int FullGameDaysToDestination = 15;

        private int _currentDayOfTravel;

        public int TravelDaysToDestination { get; private set; }

        public Party Party { get; private set; }

        public TravelNode CurrentNode { get; private set; }

        public static TravelManager Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            if (!SceneManager.GetActiveScene().name.Equals(GlobalHelper.CombatScene))
            {
                EventMediator.Instance.SubscribeToEvent(GlobalHelper.CampingEncounterFinished, this);
            }

            EventMediator.Instance.SubscribeToEvent(GlobalHelper.EntityDead, this);

            _currentDayOfTravel = 0;

            TravelDaysToDestination = DemoDaysToDestination;

            //todo testing
            //TESTING//////////////////////////////////////
            NewParty();
            //END TESTING/////////////////////////////////

            //todo this is getting called when we come back to travel scene -- was it always getting called? I don't think we tested with combat is why
            // prob don't wanna call this in start method then
            StartNewDay();
        }

        public void NewParty()
        {
            Party = new Party();
        }

        public void StartNewDay()
        {
            EncounterManager.Instance.BuildDecksForNewDay();
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

        //todo refactor
        public List<string> ApplyEncounterReward(Reward reward)
        {
            var rewardsText = new List<string>(); //todo add each reward text to this list then return. UI can handle formatting.

            if (reward.Effects != null && reward.Effects.Count > 0)
            {
                //todo apply effects
            }

            if (reward.PartyGains != null && reward.PartyGains.Count > 0)
            {
                foreach (var partyGain in reward.PartyGains)
                {
                    var gainType = (PartySupplyTypes) partyGain.Key;

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

                    rewardsText.Add(BuildPartyRewardTextItem(partyGain.Value, gainType));
                }
            }

            if (reward.EntityStatGains != null && reward.EntityStatGains.Count > 0)
            {
                foreach (var entityGain in reward.EntityStatGains)
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

                            switch (gainType)
                            {
                                case EntityStatTypes.CurrentMorale:
                                    companion.AddMorale(statGain.Value);
                                    break;
                                case EntityStatTypes.CurrentHealth:
                                    companion.AddHealth(statGain.Value);
                                    break;
                                case EntityStatTypes.CurrentEnergy:
                                    companion.AddEnergy(statGain.Value);
                                    break;
                                default:
                                    Debug.Log($"Invalid gain type! {gainType}");
                                    break;
                            }

                            rewardsText.Add(BuildCompanionRewardTextItem(companion, statGain.Value, gainType));
                        }
                    }
                }
            }

            if (reward.EntityAttributeGains != null && reward.EntityAttributeGains.Count > 0)
            {
                foreach (var entityGain in reward.EntityAttributeGains)
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

                            rewardsText.Add(BuildCompanionRewardTextItem(companion, attributeGain.Value, gainType));
                        }
                    }
                }
            }

            return rewardsText;
        }

        //todo refactor
        public List<string> ApplyEncounterPenalty(Penalty penalty)
        {
            var penaltiesText = new List<string>();

            if (penalty.Effects != null && penalty.Effects.Count > 0)
            {
                //todo apply effects
            }

            if (penalty.PartyLosses != null && penalty.PartyLosses.Count > 0)
            {
                foreach (var partyLoss in penalty.PartyLosses)
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

                    penaltiesText.Add(BuildPartyLossTextItem(partyLoss.Value, lossType));
                }
            }

            if (penalty.EntityLosses != null && penalty.EntityLosses.Count > 0)
            {
                foreach (var entityLoss in penalty.EntityLosses)
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

                            penaltiesText.Add(BuildCompanionLossTextItem(companion, statLoss.Value, lossType));
                        }
                    }
                }
            }

            return penaltiesText;
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
                    EventMediator.Instance.Broadcast(GlobalHelper.YouWon, this);
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
            }
        }
    }
}
