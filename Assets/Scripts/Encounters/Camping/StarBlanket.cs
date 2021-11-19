using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class StarBlanket : Encounter
    {
        public StarBlanket()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Star Blanket";
            Description = "The party decides to setup camp and have a decent meal around the campfire. Nothing of note takes place and they spend a peaceful night sleeping under the stars.";
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
