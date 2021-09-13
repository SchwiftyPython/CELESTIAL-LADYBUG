using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class FreeHat : Encounter
    {
        public FreeHat()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Normal;
            Title = "Free Hat!";
            Description = "The wagon slows to a stop. Derpus is knelt down for some reason. You step down to see what the hold up is and see him holding an old, dirty hat. It looks to be about three sizes too small for him.";
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            Options = new Dictionary<string, Option>();

            var optionTitle = "Let him keep it";
            var optionResultText = "Derpus grins wildly and \"wears\" the hat best he can.";

            var optionOneReward = new Reward();

            optionOneReward.AddEntityGain(travelManager.Party.Derpus, EntityStatTypes.MaxMorale, 5);

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, null, EncounterType.Normal);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Drop it!";

            optionResultText = "Derpus shrugs and tosses the hat aside. Oh well.";

            var optionTwo = new Option(optionTitle, optionResultText, null, null, EncounterType.Normal);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
