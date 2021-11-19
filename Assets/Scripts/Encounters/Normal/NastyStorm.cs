using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class NastyStorm : Encounter
    {
        public NastyStorm()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Nasty Storm";
            Description = "The group gets caught in a sudden downpour! Thunder crashes around them as they look for a place to take shelter, but there is none! With no other options, they are forced to keep moving through the elemental onslaught.";
            ImageResultName = "storm";
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            Penalty = new Penalty();
            Penalty.AddEntityLoss(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

            foreach (var companion in travelManager.Party.GetCompanions())
            {
                Penalty.AddEntityLoss(companion, EntityStatTypes.CurrentMorale, 10);
            }

            var fullResultDescription = new List<string> {Description + "\n"};

            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
