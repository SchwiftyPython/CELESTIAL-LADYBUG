using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Travel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Encounters
{
    public abstract class Encounter : ISubscriber
    {
        private const string OptionSelectedEvent = GlobalHelper.EncounterOptionSelected;

        public Rarity Rarity;

        public EncounterType EncounterType;

        public string Title;

        public string Description;

        public Dictionary<string, Option> Options;

        public Reward Reward;

        public Penalty Penalty;

        public bool CountsAsDayTraveled;

        public List<BiomeType> BiomeTypes;

        public string ImageName;
        public string ImageResultName;

        public Party Party
        {
            get
            {
                var travelManager = Object.FindObjectOfType<TravelManager>();

                return travelManager.Party;
            }
        }

        public abstract void Run();

        public void OptionSelected(Option selectedOption)
        {
            if (selectedOption == null)
            {
                Debug.Log("No option selected for Encounter!");

                throw new ArgumentNullException(nameof(selectedOption));
            }

            var travelManager = Object.FindObjectOfType<TravelManager>();

            if (selectedOption.HasReward())
            {
                travelManager.ApplyEncounterReward(selectedOption.Reward);
            }

            if (selectedOption.HasPenalty())
            {
                travelManager.ApplyEncounterPenalty(selectedOption.Penalty);
            }

            var fullResultDescription = new List<string> {selectedOption.ResultText + "\n"};

            EncounterType = selectedOption.TargetEncounterType;

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            if (EncounterType == EncounterType.Combat)
            {
                if (selectedOption is RetreatCombatOption retreatCombatOption)
                {
                    if (retreatCombatOption.Success)
                    {
                        eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
                    }
                    else
                    {
                        SubscribeToOptionSelectedEvent();
                        eventMediator.Broadcast(GlobalHelper.RetreatEncounterFailed, this, fullResultDescription);
                    }
                }
                else if(selectedOption is FightCombatOption fightCombatOption)
                {
                    if (!string.IsNullOrEmpty(fightCombatOption.ResultText))
                    {
                        eventMediator.Broadcast(GlobalHelper.ShowCombatPreview, this, fightCombatOption);
                    }
                    else
                    {
                        SceneManager.LoadScene(GlobalHelper.CombatScene);

                        var combatManager = Object.FindObjectOfType<CombatManager>();

                        combatManager.Enemies = ((FightCombatOption)selectedOption).Enemies;

                        combatManager.LoadCombatScene();
                    }
                }
            }
            else if (fullResultDescription.Count > 1 || !fullResultDescription.First().Equals("\n"))
            {
                eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
            }
            else
            {
                eventMediator.Broadcast(GlobalHelper.EncounterFinished, this);
            }
        }

        public bool HasOptions()
        {
            return Options != null && Options.Count > 0;
        }

        public bool ValidBiome(BiomeType bType)
        {
            if (BiomeTypes == null || BiomeTypes.Count < 1)
            {
                return true;
            }

            return BiomeTypes.Contains(bType);
        }

        protected void SubscribeToOptionSelectedEvent()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(OptionSelectedEvent, this);
        }

        protected void UnsubscribeFromOptionSelectedEvent()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.UnsubscribeFromEvent(OptionSelectedEvent, this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(OptionSelectedEvent))
            {
                if (!HasOptions())
                {
                    return;
                }

                var optionName = parameter.ToString();

                if (!Options.ContainsKey(optionName))
                {
                    //todo maybe throw exception
                    return;
                }

                OptionSelected(Options[optionName]);

                UnsubscribeFromOptionSelectedEvent();
            }
        }
    }
}
