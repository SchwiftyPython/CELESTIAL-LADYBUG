using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class HotSpringRelaxin : Encounter
    {
        public HotSpringRelaxin()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Hot Spring Relaxin'";
            Description = "An upthrust of sparkling white rock stands at the edge of a small clearing. Clear water flows from the top of the stone, filling a natural pool stippled with luminous green lilies.";
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            var optionTitle = "Take a dip";

            var optionResultText =
                "It's been ages since anyone properly bathed! The party takes turns in the spring. It's a truly relaxing experience and some people's wounds were healed! Must be some magic in them springs!";

            var optionOneReward = new Reward();

            var travelManager = Object.FindObjectOfType<TravelManager>();

            optionOneReward.EveryoneGain(travelManager.Party, EntityStatTypes.CurrentMorale, 10);
            optionOneReward.EveryoneGain(travelManager.Party, EntityStatTypes.CurrentHealth, 5);

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, null, EncounterType.Normal);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Keep moving";

            optionResultText = "Baths shmaths! Our smell is natural!";

            var optionTwo = new Option(optionTitle, optionResultText, null, null, EncounterType.Normal);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);

            //todo blurb companion saying we should have stopped cause so and so reeks
        }
    }
}
