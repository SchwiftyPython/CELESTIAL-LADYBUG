using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class SweetTooth : Encounter
    {
        public SweetTooth()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Sweet Tooth";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            var gold = travelManager.Party.Gold;

            const int pastryCost = 30;

            const int foodGain = 9;

            Reward = new Reward();
            Penalty = new Penalty();

            var fatty = travelManager.Party.GetRandomCompanion();

            Description = $"The party decides to camp outside of a small village. {fatty.FirstName()} disappears for awhile to check things out. When they return, they're carrying an armful of pastries. ";

            if (gold <= 0)
            {
                Description += $"\n\nThey found them in a basket with no one around to claim them!";
            }
            else if (gold <= pastryCost)
            {
                Description += $"\n\nThey spent the rest of the gold!";

                Reward.AddPartyGain(PartySupplyTypes.Food, foodGain - 2);

                Penalty.AddPartyLoss(PartySupplyTypes.Gold, gold);
            }
            else
            {
                Description += $"\n\nThey spent {pastryCost} gold!";

                Reward.AddPartyGain(PartySupplyTypes.Food, foodGain);

                Penalty.AddPartyLoss(PartySupplyTypes.Gold, pastryCost);
            }

            Description += $"\n\nThe pastries are pretty good though and there are plenty leftover after everyone stuffs their faces!";

            Reward.EveryoneGain(travelManager.Party, EntityStatTypes.CurrentEnergy, 10);
            Reward.EveryoneGain(travelManager.Party, EntityStatTypes.CurrentMorale, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            travelManager.ApplyEncounterPenalty(Penalty);
            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
