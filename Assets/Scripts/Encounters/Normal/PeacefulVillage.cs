using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class PeacefulVillage : Encounter
    {
        public PeacefulVillage()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Peaceful Village";
            Description = "The party wanders into a peaceful village. Everyone is going about their business and pay the adventurers little mind.";
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            var optionTitle = "Scavenge for supplies (free)";

            var optionResultText = "Everyone splits up and scrounges up what they can from the streets and alleys.";

            var optionReward = new Reward();

            var foodGained = Random.Range(4, 9);

            optionReward.AddPartyGain(PartySupplyTypes.Food, foodGained);

            const int potionChance = 50;

            //todo diceroller
            var roll = Random.Range(1, 101);

            int potionsGained;
            if (roll <= potionChance)
            {
                potionsGained = Random.Range(1, 6);

                optionReward.AddPartyGain(PartySupplyTypes.HealthPotions, potionsGained);
            }

            const int goldChance = 30;

            //todo diceroller
            roll = Random.Range(1, 101);

            if (roll <= goldChance)
            {
                var goldGained = Random.Range(20, 41);

                optionReward.AddPartyGain(PartySupplyTypes.Gold, goldGained);
            }

            var optionOne = new Option(optionTitle, optionResultText, optionReward, null, EncounterType);

            Options.Add(optionTitle, optionOne);

            var supplyCost = Random.Range(25, 81);

            optionTitle = $"Buy supplies ({supplyCost} gold)";
            optionResultText = "A local shopkeeper is happy to do business.";

            optionReward = new Reward();

            foodGained *= 2;

            optionReward.AddPartyGain(PartySupplyTypes.Food, foodGained);

            potionsGained = Random.Range(2, 12);

            optionReward.AddPartyGain(PartySupplyTypes.HealthPotions, potionsGained);

            var optionPenalty = new Penalty();

            optionPenalty.AddPartyLoss(PartySupplyTypes.Gold, supplyCost);

            var optionTwo = new Option(optionTitle, optionResultText, optionReward, optionPenalty, EncounterType);

            Options.Add(optionTitle, optionTwo);

            var travelManager = Object.FindObjectOfType<TravelManager>();

            var restCost = travelManager.Party.GetCompanions().Count * 10;

            if (travelManager.Party.Gold >= restCost)
            {
                optionTitle = $"Rest for a day ({restCost} gold)";
                optionResultText = $"The party stays at the local inn and gets some good shut-eye.";

                optionReward = new Reward();

                optionReward.AddEntityGain(travelManager.Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

                foreach (var companion in travelManager.Party.GetCompanions())
                {
                    optionReward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
                }

                optionReward.AddEntityGain(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

                foreach (var companion in travelManager.Party.GetCompanions())
                {
                    optionReward.AddEntityGain(companion, EntityStatTypes.CurrentMorale, 10);
                }

                optionPenalty = new Penalty();

                optionPenalty.AddPartyLoss(PartySupplyTypes.Gold, restCost);

                EncounterType = EncounterType.Camping;

                CountsAsDayTraveled = false;

                var optionThree = new Option(optionTitle, optionResultText, optionReward, optionPenalty, EncounterType);

                Options.Add(optionTitle, optionThree);
            }

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
