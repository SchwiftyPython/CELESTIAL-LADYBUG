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

        //todo refactor
        public void ApplyEncounterReward(Reward reward)
        {
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
                    }
                }
            }
        }
    }
}
