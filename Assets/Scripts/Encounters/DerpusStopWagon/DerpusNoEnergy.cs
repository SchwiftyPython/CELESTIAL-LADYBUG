using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.DerpusStopWagon
{
    public class DerpusNoEnergy : Encounter
    {
        public DerpusNoEnergy()
        {
            EncounterType = EncounterType.Camping;
            Title = "Derpus Tired";
            Description =
                "The wagon slowly rolls to a stop. Derpus is too tired and has decided to nap the rest of the day.";
            CountsAsDayTraveled = false;
        }

        public override void Run()
        {
            Reward = new Reward();

            var travelManager = Object.FindObjectOfType<TravelManager>();
            Reward.AddEntityGain(travelManager.Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in travelManager.Party.GetCompanions())
            {
                Reward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
