using System;
using System.Collections.Generic;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters.Normal
{
    public class CauldronRoulette : Encounter
    {
        public CauldronRoulette()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Cauldron Roulette";
        }

        public override void Run()
        {
            var curiousCompanion = TravelManager.Instance.Party.GetRandomCompanion();

            Description = "The party happens upon an odd sight: A bubbling cauldron smack dab in the middle of a clearing. \n\n";
            Description += $"Four items sit on a table next to the cauldron. {curiousCompanion.Name} thinks they should cast one of the items in. They approach the cauldron and toss in: ";

            Options = new Dictionary<string, Option>();

            var optionTitle = "A Steak";
            var optionResultText = "The cauldron glows and a disembodied voice bellows:\n\n";
            optionResultText += "PROTEIN PROTEIN PROTEIN PROTEIN";

            var optionReward = new Reward();
            Penalty optionPenalty = null;

            optionReward.AddEntityGain(curiousCompanion, EntityAttributeTypes.Might, 1);

            var optionOne = new Option(optionTitle, optionResultText, optionReward, null, EncounterType);

            Options.Add(optionTitle, optionOne);

            optionTitle = "A Vial of Water";
            optionResultText = "The cauldron explodes showering everyone with hot liquid!";

            optionPenalty = new Penalty();

            foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            {
                optionPenalty.AddEntityLoss(companion, EntityStatTypes.CurrentHealth, 5);
            }

            var optionTwo = new Option(optionTitle, optionResultText, null, optionPenalty, EncounterType);

            Options.Add(optionTitle, optionTwo);

            optionTitle = "A Pair of Rat Legs";
            optionResultText = $"The cauldron's bubbling grows violent and a discolored cloud envelopes {curiousCompanion.FirstName()}. The cloud clears and they feel... different. \n\n Faster? \n\n Faster.";

            optionReward = new Reward();
            optionReward.AddEntityGain(curiousCompanion, EntityAttributeTypes.Speed, 1);

            var optionThree = new Option(optionTitle, optionResultText, optionReward, null, EncounterType);

            Options.Add(optionTitle, optionThree);

            optionTitle = "A Shopping List";
            optionResultText = $"{curiousCompanion.FirstName()} drops the shopping list into the cauldron. Supplies appear out of thin air next to the wagon!";

            optionReward = new Reward();
            optionReward.AddPartyGain(PartySupplyTypes.Food, 15);
            optionReward.AddPartyGain(PartySupplyTypes.HealthPotions, 3);

            var optionFour = new Option(optionTitle, optionResultText, optionReward, null, EncounterType);

            Options.Add(optionTitle, optionFour);

            SubscribeToOptionSelectedEvent();

            EventMediator.Instance.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
