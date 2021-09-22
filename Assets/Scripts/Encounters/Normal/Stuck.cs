using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class Stuck : Encounter
    {
        public Stuck()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Stuck!";
        }

        public override void Run()
        {
            Description = "The wagon gets stuck in some kind of mud. It takes a couple hours to get it moving again. Everyone is dirty and irritated by the end of it.";

            Penalty = new Penalty();
            Penalty.EveryoneLoss(Party, EntityStatTypes.CurrentMorale, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
