using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters
{
    public class ComfyInn : Encounter
    {
        private const int CostPerPerson = 10;
        private const int EnergyGain = 50;
        private const int BreakfastChance = 2;

        public ComfyInn()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "Comfy Inn";
            Description = $"A little inn pops up on the road as the sun is setting. A little old lady runs the place and says it'll cost {CostPerPerson} gold each to stay the night.";

            Options = new Dictionary<string, Option>();

            var totalCost = CostPerPerson * TravelManager.Instance.Party.GetCompanions().Count + 1;

            string optionTitle;
            string optionResultText;
            if (totalCost <= TravelManager.Instance.Party.Gold)
            {
                optionTitle = $"Pay the {totalCost} gold";
                optionResultText = $"The group pays {totalCost} gold to stay the night. It's pretty comfy!";

                var entityGains = new Dictionary<Entity, KeyValuePair<object, int>>
                {
                    {TravelManager.Instance.Party.Derpus, new KeyValuePair<object, int>("energy", EnergyGain)}
                };

                foreach (var companion in TravelManager.Instance.Party.GetCompanions())
                {
                    entityGains.Add(companion, new KeyValuePair<object, int>("energy", EnergyGain));
                }

                var optionOnePenalty = new Penalty();
                optionOnePenalty.AddPartyLoss(PartySupplyTypes.Gold, totalCost);

                var rollForBreakfast = Random.Range(0, 101); //todo diceroller

                Dictionary<object, int> partyGains = null;
                if (rollForBreakfast <= BreakfastChance)
                {
                    optionResultText = $"\nThe innkeeper serves breakfast as thanks for being great guests!";

                    entityGains.Add(TravelManager.Instance.Party.Derpus, new KeyValuePair<object, int>("morale", 25));

                    foreach (var companion in TravelManager.Instance.Party.GetCompanions())
                    {
                        entityGains.Add(companion, new KeyValuePair<object, int>("morale", 25));
                    }

                    partyGains = new Dictionary<object, int>
                    {
                        {"food", TravelManager.Instance.Party.GetCompanions().Count * Party.FoodConsumedPerCompanion}
                    };
                }

                var optionReward = new Reward(entityGains, partyGains);

                Options.Add(optionTitle, new Option(optionTitle, optionResultText, optionReward, optionOnePenalty));
            }

            optionTitle = "No thanks";
            optionResultText = "You keep on going and travel through the night.";

            var optionTwoPenalty = new Penalty();
            optionTwoPenalty.AddEntityLoss(TravelManager.Instance.Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            {
                optionTwoPenalty.AddEntityLoss(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            Options.Add(optionTitle, new Option(optionTitle, optionResultText, null, optionTwoPenalty));
        }

        public override void Run()
        {
            SubscribeToOptionSelectedEvent();

            EventMediator.Instance.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
