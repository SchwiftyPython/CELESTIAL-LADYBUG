using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class ItsWhatsForDinner : Encounter
    {
        private const int FoodPerPerson = 5;

        public ItsWhatsForDinner()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "It's What's For Dinner";
            Description = "Another day on the trail until you find an entire deep fried cow just left in the ditch! It's still warm and everyone decides that the thirty minute rule is in effect and takes what they can!";
        }

        public override void Run()
        {
            Reward = new Reward();

            Reward.AddPartyGain(PartySupplyTypes.Food, Party.Size * FoodPerPerson);
            Reward.EveryoneGain(Party, EntityStatTypes.CurrentMorale, 15);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
