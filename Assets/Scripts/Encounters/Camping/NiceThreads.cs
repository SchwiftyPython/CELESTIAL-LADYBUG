using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class NiceThreads : Encounter
    {
        public NiceThreads()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "Nice Threads!";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            var gold = travelManager.Party.Gold;

            const int clothesCost = 50;

            Reward = new Reward();
            Penalty = new Penalty();

            var trendy = travelManager.Party.GetRandomCompanion();

            Description = $"The party decides to camp outside of a town known for its shopping district. {trendy.FirstName()} disappears for awhile to check things out. They return proudly wearing the poofiest, silkiest pantaloons you've ever seen!";

            if (gold <= 0)
            {
                Description += $"\n\nThey found the clothes hanging out to dry in someone's yard and took them!";
            }
            else
            if (gold <= clothesCost)
            {
                Description += $"\n\nThey spent the rest of the gold!";

                Penalty.AddPartyLoss(PartySupplyTypes.Gold, gold);
            }
            else
            {
                Description += $"\n\nThey spent {clothesCost} gold!";

                Penalty.AddPartyLoss(PartySupplyTypes.Gold, clothesCost);
            }

            Reward.EveryoneGain(travelManager.Party, EntityStatTypes.CurrentEnergy, 10);

            Reward.AddEntityGain(trendy, EntityStatTypes.CurrentMorale, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            travelManager.ApplyEncounterPenalty(Penalty);
            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
