using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class BadHaircut : Encounter
    {
        public BadHaircut()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Bad Haircut";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            var chosen = Party.GetRandomCompanion();

            Description =
                $"The party makes camp and it's a pretty uneventful evening until {chosen.FirstName()} decides to cut their own hair. It turns out awful!";

            Reward = new Reward();

            Reward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 10);

            Penalty = new Penalty();

            Penalty.AddEntityLoss(chosen, EntityStatTypes.CurrentMorale, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterPenalty(Penalty);
            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
