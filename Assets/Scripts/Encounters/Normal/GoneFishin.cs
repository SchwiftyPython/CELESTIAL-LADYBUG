using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class GoneFishin : Encounter
    {
        public GoneFishin()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.MentalBreak;
            Title = "Gone Fishin'";
        }

        public override void Run()
        {
            var fisher = Party.GetRandomCompanion();

            Description = $"The trail follows along side a small river and {fisher.FirstName()} thinks they can catch some fish to fry up later. They wade out a bit and manage to catch a few fish by hand!";

            Reward = new Reward();

            Reward.AddPartyGain(PartySupplyTypes.Food, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
