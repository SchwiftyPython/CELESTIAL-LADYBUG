using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class FortyHands : Encounter
    {
        public FortyHands()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Camping;
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            var partyPal = Party.GetRandomCompanion();

            Title = $"{partyPal.FirstName()} Forty Hands";

            Description = $"The party camps outside a town known for its taverns. In the morning, {partyPal.FirstName()} is missing! After a short search, they are found in a nearby field in their underwear with empty bottles tied to their hands.";

            const int equipmentLostChance = 18;

            var roll = Dice.Roll("1d100");

            if (roll <= equipmentLostChance)
            {
                partyPal.UnEquipAll();

                Description += "\n\nTheir equipment is nowhere to be found!";
            }
            else
            {
                Description += "\n\nLuckily, their equipment is found close by.";
            }

            Penalty = new Penalty();

            Penalty.AddEntityLoss(partyPal, EntityStatTypes.CurrentEnergy, 10);

            Reward = new Reward();

            Reward.AddEntityGain(Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in Party.GetCompanions())
            {
                if (companion == partyPal)
                {
                    continue;
                }

                Reward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterPenalty(Penalty);
            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
