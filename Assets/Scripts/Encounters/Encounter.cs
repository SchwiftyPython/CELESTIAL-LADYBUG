using System;
using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

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

        public abstract void Run();

        public void OptionSelected(Option selectedOption)
        {
            if (selectedOption == null)
            {
                Debug.Log("No option selected for Stay In School Encounter!");

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

            var fullResultDescription = new List<string>(); 

            fullResultDescription.Add(selectedOption.ResultText + "\n");
            if (rewardsText != null)
            {
                fullResultDescription.AddRange(rewardsText);
            }

            if (penaltiesText != null)
            {
                fullResultDescription.AddRange(penaltiesText);
            }

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
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
