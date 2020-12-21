using System.Collections.Generic;
using Assets.Scripts.Encounters;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Travel
{
    public class TravelManager : MonoBehaviour
    {
        public Party Party { get; private set; }

        public TravelNode CurrentNode { get; private set; }

        public static TravelManager Instance;

        private void Awake()
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
        }

        public void NewParty()
        {
            Party = new Party();
        }

        public string BuildPartyRewardTextItem(int value, string gainType)
        {
            return $"Gained {value} {gainType}!";
        }

        public string BuildPartyLossTextItem(int value, string lossType)
        {
            return $"Lost {value} {lossType}!";
        }

        public string BuildCompanionRewardTextItem(Entity companion, int value, string gainType)
        {
            return $"{companion.Name} gained {value} {gainType}!";
        }

        public string BuildCompanionLossTextItem(Entity companion, int value, string lossType)
        {
            return $"{companion.Name} lost {value} {lossType}!";
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
                    var gainType = partyGain.Key.ToString();

                    switch (gainType)
                    {
                        case "food":
                            Party.Food += partyGain.Value;
                            break;
                        case "potions":
                            Party.HealthPotions += partyGain.Value;
                            break;
                        case "gold":
                            Party.Gold += partyGain.Value;
                            break;
                        default:
                            Debug.Log($"Invalid gain type! {gainType}");
                            break;
                    }

                    rewardsText.Add(BuildPartyRewardTextItem(partyGain.Value, gainType));
                }
            }

            if (reward.EntityGains != null && reward.EntityGains.Count > 0)
            {
                foreach (var entityGain in reward.EntityGains)
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
                        var gainType = entityGain.Value.Key.ToString();

                        switch (gainType)
                        {
                            case "morale":
                                companion.AddMorale(entityGain.Value.Value);
                                break;
                            case "health":
                                companion.AddHealth(entityGain.Value.Value);
                                break;
                            case "energy":
                                companion.AddEnergy((entityGain.Value.Value));
                                break;
                            default:
                                Debug.Log($"Invalid gain type! {gainType}");
                                break;
                        }

                        rewardsText.Add(BuildCompanionRewardTextItem(companion, entityGain.Value.Value, gainType));
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
                    var lossType = partyLoss.Key.ToString();

                    switch (lossType)
                    {
                        case "food":
                            Party.Food -= partyLoss.Value;
                            break;
                        case "potions":
                            Party.HealthPotions -= partyLoss.Value;
                            break;
                        case "gold":
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
                        var lossType = entityLoss.Value.Key.ToString();

                        switch (lossType)
                        {
                            case "morale":
                                companion.SubtractMorale(entityLoss.Value.Value);
                                break;
                            case "health":
                                companion.SubtractHealth(entityLoss.Value.Value);
                                break;
                            case "energy":
                                companion.SubtractEnergy((entityLoss.Value.Value));
                                break;
                            default:
                                Debug.Log($"Invalid gain type! {lossType}");
                                break;
                        }

                        penaltiesText.Add(BuildCompanionLossTextItem(companion, entityLoss.Value.Value, lossType));
                    }
                }
            }

            return penaltiesText;
        }
    }
}
