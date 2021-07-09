using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.DerpusStopWagon
{
    public class DerpusNoMorale : Encounter
    {
        public DerpusNoMorale()
        {
            EncounterType = EncounterType.Camping;
            Title = "Derpus Sad";
            Description =
                "The wagon slowly rolls to a stop. Derpus droops to a sad squat and refuses to move for the rest of the day.";
            CountsAsDayTraveled = false;
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();
            var derpus = travelManager.Party.Derpus;

            Reward = new Reward();

            Reward.AddEntityGain(derpus, EntityStatTypes.CurrentEnergy, derpus.Stats.MaxMorale / 2);

            Reward.AddEntityGain(derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in travelManager.Party.GetCompanions())
            {
                Reward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            var rewardsText = travelManager.ApplyEncounterReward(Reward);

            fullResultDescription.AddRange(rewardsText);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
