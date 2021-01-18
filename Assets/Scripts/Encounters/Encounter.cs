using System;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Travel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Encounters
{
    public abstract class Encounter: ISubscriber
    {
        private const string OptionSelectedEvent = GlobalHelper.EncounterOptionSelected;

        public Rarity Rarity;

        public EncounterType EncounterType;

        public string Title;

        public string Description;

        public Dictionary<string, Option> Options;

        public Reward Reward;

        public Penalty Penalty;

        public abstract void Run();

        public void OptionSelected(Option selectedOption)
        {
            if (selectedOption == null)
            {
                Debug.Log("No option selected for Encounter!");

                throw new ArgumentNullException(nameof(selectedOption));
            }

            List<string> rewardsText = null; 
            if (selectedOption.HasReward())
            {
                rewardsText = TravelManager.Instance.ApplyEncounterReward(selectedOption.Reward);
            }

            List<string> penaltiesText = null; 
            if (selectedOption.HasPenalty())
            {
                penaltiesText = TravelManager.Instance.ApplyEncounterPenalty(selectedOption.Penalty);
            }

            var fullResultDescription = new List<string> {selectedOption.ResultText + "\n"};

            if (rewardsText != null)
            {
                fullResultDescription.AddRange(rewardsText);
            }

            if (penaltiesText != null)
            {
                fullResultDescription.AddRange(penaltiesText);
            }

            if (EncounterType == EncounterType.Combat)
            {
                if (selectedOption is RetreatCombatOption retreatCombatOption)
                {
                    if (retreatCombatOption.Success)
                    {
                        EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
                    }
                    else
                    {
                        SubscribeToOptionSelectedEvent();
                        EventMediator.Instance.Broadcast(GlobalHelper.RetreatEncounterFailed, this, fullResultDescription);
                    }
                }
                else //if to arms option selected
                {
                    SceneManager.LoadScene(GlobalHelper.CombatScene);

                    CombatManager.Instance.Enemies = ((FightCombatOption) selectedOption).Enemies;
                }
               
            }
            else
            {
                EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
            }
        }

        public bool HasOptions()
        {
            return Options != null && Options.Count > 0;
        }

        protected void SubscribeToOptionSelectedEvent()
        {
            EventMediator.Instance.SubscribeToEvent(OptionSelectedEvent, this);
        }

        protected void UnsubscribeFromOptionSelectedEvent()
        {
            EventMediator.Instance.UnsubscribeFromEvent(OptionSelectedEvent, this);
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
