using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class AwfulSick : Encounter
    {
        public AwfulSick()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Camping;
            Title = "Awful Sick";
            Description =
                "Everyone settles in for the night under the stars. It is quite peaceful until one by one, everyone starts vomiting up the Orc Tuna Special that Derpus threw together for dinner.";
            CountsAsDayTraveled = true;
            ImageResultName = "throw up";
        }

        public override void Run()
        {
            Reward = new Reward();

            Reward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 5);

            Penalty = new Penalty();

            Penalty.EveryoneLoss(Party, EntityStatTypes.CurrentMorale, 5);

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterReward(Reward);
            travelManager.ApplyEncounterPenalty(Penalty);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
