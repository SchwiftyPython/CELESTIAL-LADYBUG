using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.MentalBreak
{
    public class DazedMugging : Encounter
    {
        private readonly Entity _companion;

        public DazedMugging(Entity dazedCompanion)
        {
            _companion = dazedCompanion;
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.MentalBreak;
            Title = "Dazed Mugging";
            Description = $"{dazedCompanion.Name} wanders away from the group in a daze and is mugged by a roving band of ne'er-do-wells.";
        }

        public override void Run()
        {
            Penalty = new Penalty();
            Penalty.AddEntityLoss(_companion, EntityStatTypes.CurrentHealth, 10);

            Penalty.AddPartyLoss(PartySupplyTypes.Gold, 35);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);

            _companion.Stats.CurrentMorale = _companion.Stats.MaxMorale;
        }
    }
}
