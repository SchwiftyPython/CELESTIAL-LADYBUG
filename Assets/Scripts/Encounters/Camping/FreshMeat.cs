using System.Collections.Generic;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class FreshMeat : Encounter
    {
        public FreshMeat()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "Fresh Meat";
            CountsAsDayTraveled = true;
        }

        public override void Run()  //todo possible continuity
        {
            var chosen = Party.GetRandomCompanion();

            Description =
                $"The party is setting up camp when a group of traveling lizardmen approach. They would like to {chosen.FirstName()} as food.\n\nThey offer 125 gold!";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Agree for 125 gold";

            var optionResultText = $"You turn {chosen.Name} over to the lizardmen. Your companions are horrified!";

            var optionOnePenalty = new Penalty();

            optionOnePenalty.RemoveFromParty(chosen);

            optionOnePenalty.EveryoneLoss(Party, EntityStatTypes.CurrentMorale, 15);

            var optionOneReward = new Reward();

            optionOneReward.AddPartyGain(PartySupplyTypes.Gold, 125);

            optionOneReward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 10);

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, optionOnePenalty,
                EncounterType.Camping);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Counter with 200 gold";

            const int success = 33;

            var roll = Dice.Roll("1d100");

            var optionTwoPenalty = new Penalty();

            var optionTwoReward = new Reward();

            if (roll <= success)
            {
                optionResultText = $"They discuss the price briefly and agree. You turn {chosen.Name} over to the lizardmen. Your companions are horrified!";

                optionTwoPenalty.RemoveFromParty(chosen);

                optionTwoPenalty.EveryoneLoss(Party, EntityStatTypes.CurrentMorale, 15);

                optionTwoReward.AddPartyGain(PartySupplyTypes.Gold, 200);

                optionTwoReward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 10);
            }
            else
            {
                optionResultText = $"They decline and continue on their way hungry and disappointed. Your companions are horrified! {chosen.FirstName()} is especially angry that you tried to sell them like a piece of beef!";

                var chosenMorale = chosen.Stats.CurrentMorale;

                if (chosenMorale - 20 < 0)
                {
                    optionResultText += "They stomp off and don't come back!";

                    optionTwoPenalty.RemoveFromParty(chosen);
                }

                optionTwoPenalty.EveryoneLoss(Party, EntityStatTypes.CurrentMorale, 15);

                optionTwoReward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 10);
            }

            var optionTwo = new Option(optionTitle, optionResultText, optionTwoReward, optionTwoPenalty,
                EncounterType.Camping);

            Options.Add(optionTitle, optionTwo);

            optionTitle = "Uhh no";

            optionResultText = $"The lizardmen continue on their way hungry and disappointed. {chosen.FirstName()} is looking themselves up and down trying to figure out why they were picked. They can't decide if they should be worried or flattered.";

            var optionThreeReward = new Reward();

            optionThreeReward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 10);

            var optionThree = new Option(optionTitle, optionResultText, optionThreeReward, null, EncounterType.Camping);

            Options.Add(optionTitle, optionThree);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
