using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class CampMosquito : Encounter
    {
        public CampMosquito()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Camp Mosquito";
            Description =
                "The group happens upon an old campsite. During the night, they are attacked by swarms of thirsty mosquitoes and must leave before shutting an eye!";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            Penalty = new Penalty();
            Penalty.AddEntityLoss(travelManager.Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in travelManager.Party.GetCompanions())
            {
                Penalty.AddEntityLoss(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var fullResultDescription = new List<string> {Description + "\n"};

            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
