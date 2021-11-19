using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class ConkOut : Encounter
    {
        public ConkOut()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "Conk out";
            Description = "After a particularly long day on the trail, the weary crew sets up camp and falls asleep instantly.";
            CountsAsDayTraveled = true;
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
