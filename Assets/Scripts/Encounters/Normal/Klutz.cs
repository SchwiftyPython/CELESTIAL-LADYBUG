using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class Klutz : Encounter
    {
        public Klutz()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Klutz";
        }

        public override void Run()
        {
            var klutz = Party.GetRandomCompanion();

            Description = $"While walking along a riverbank, the ground gives out from under {klutz.FirstName()}'s feet and they slide into the water. Some of their gear is lost or ruined!";

            Penalty = new Penalty();

            Penalty.AddEntityLoss(klutz, EntityStatTypes.CurrentMorale, 10);

            Penalty.AddPartyLoss(PartySupplyTypes.Food, 2);
            Penalty.AddPartyLoss(PartySupplyTypes.HealthPotions, 2);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
