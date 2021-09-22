using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class Bummer : Encounter
    {
        public Bummer()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Bummer";
            Description = "While traveling the trail on a particularly long stint Derpus spots a Big Mama's Tacos! No way they have one of those out here! But when the wagon gets closer it turns out it was just a big tree stump.";
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            Penalty = new Penalty();

            Penalty.EveryoneLoss(travelManager.Party, EntityStatTypes.CurrentMorale, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);

            //todo blurb about are they sure we're going the right way if he can't see where hes going
        }
    }
}
