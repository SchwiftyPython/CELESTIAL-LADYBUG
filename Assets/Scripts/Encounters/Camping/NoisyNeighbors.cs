using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class NoisyNeighbors : Encounter
    {
        public NoisyNeighbors()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "Noisy Neighbors";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            Description =
                "The party stays at a small campground with an assortment of travelers. Safety in numbers of course. Everyone gets along pretty well, but a band of farmers gets way too drunk and keeps playing their new single 'Tilling in the Name' well into the night!";

            Reward = new Reward();

            Reward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
