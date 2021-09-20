using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.MentalBreak
{
    public class AbandonParty : Encounter
    {
        private readonly Entity _companion;

        public AbandonParty(Entity quitter)
        {
            _companion = quitter;
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.MentalBreak;
            Title = $"{quitter.Name} Quits!";
            Description = $"{quitter.Name} has had enough and leaves when no one is paying attention.";
        }

        public override void Run()
        {
            Penalty = new Penalty();

            Penalty.RemoveFromParty(_companion);

            const int theftChance = 54;

            var roll = Dice.Roll("1d100");

            if (roll <= theftChance)
            {
                Description += "\n\nThey stole some supplies too!";
            }

            const int foodChance = 51;

            if (roll <= foodChance)
            {
                Penalty.AddPartyLoss(PartySupplyTypes.Food, Party.Food >= 10 ? 10 : Party.Food);
            }

            const int goldChance = 5;

            if (roll <= goldChance)
            {
                Penalty.AddPartyLoss(PartySupplyTypes.Gold, Party.Gold >= 70 ? 70 : Party.Gold);
            }

            const int potionChance = 10;

            if (roll <= potionChance)
            {
                Penalty.AddPartyLoss(PartySupplyTypes.Gold, Party.HealthPotions >= 2 ? 2 : Party.HealthPotions);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
