using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.MentalBreak
{
    public class NervousNellie : Encounter
    {
        private readonly Entity _companion;

        public NervousNellie(Entity shakyBoi)
        {
            _companion = shakyBoi;
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.MentalBreak;
            Title = $"Nervous {shakyBoi.Name}!";
            Description = $"All of the stress starts to wear on {shakyBoi.Name}. They become permanently anxious!";
        }

        public override void Run()
        {
            Penalty = new Penalty();

            Penalty.AddEntityLoss(_companion, EntitySkillTypes.Melee, 1);
            Penalty.AddEntityLoss(_companion, EntitySkillTypes.Ranged, 1);
            Penalty.AddEntityLoss(_companion, EntitySkillTypes.Survival, 1);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterPenalty(Penalty);

            _companion.Stats.CurrentMorale = _companion.Stats.MaxMorale;

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
