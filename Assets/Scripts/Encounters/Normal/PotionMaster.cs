using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class PotionMaster : Encounter
    {
        public PotionMaster()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Potion Master";
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();
            var chosenCompanion = travelManager.Party.GetRandomCompanion();

            Description =
                $"{chosenCompanion.FirstName()} finds an abandoned cabin. There are some supplies inside. They are about to leave when they notice a trapdoor to an underground lab. Looks like some unfinished health potions were left behind. Handling these could be dangerous!";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Leave them alone!";

            string optionResultText = "Everyone agrees it's not worth the risk and leaves the lab untouched.";

            var optionOneReward = new Reward();

            optionOneReward.AddPartyGain(PartySupplyTypes.Food, 6);

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, null, EncounterType);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Finish making them";

            Reward optionTwoReward = null;
            Penalty optionTwoPenalty = null;

            const int finishSuccess = 20;

            var bestCoord = travelManager.Party.GetCompanionWithHighestCoordination();

            var coordCheck = Dice.Roll($"{bestCoord.Attributes.Coordination - 1}d6");

            var wildRoll = GlobalHelper.RollWildDie();

            coordCheck += wildRoll;

            if (bestCoord.Attributes.Coordination > 5)
            {
                optionResultText = $"{chosenCompanion.FirstName()} attempts to finish the potions. \n\nThey finish them easily and end up with 5 potions.";

                optionTwoReward = new Reward();
                optionOneReward.AddPartyGain(PartySupplyTypes.Food, 6);
                optionTwoReward.AddPartyGain(PartySupplyTypes.HealthPotions, 5);
            }
            else if (coordCheck >= finishSuccess)
            {
                optionResultText = $"{chosenCompanion.FirstName()} attempts to finish the potions. \n\nThey ruin a few of the potions, but nobody gets hurt.";

                optionTwoReward = new Reward();
                optionOneReward.AddPartyGain(PartySupplyTypes.Food, 6);
                optionTwoReward.AddPartyGain(PartySupplyTypes.HealthPotions, 3);
            }
            else
            {
                optionResultText = $"{chosenCompanion.FirstName()} attempts to finish the potions. \n\nThey are in over their head and make a horrible mistake!";

                optionTwoPenalty = new Penalty();

                foreach (var companion in travelManager.Party.GetCompanions())
                {
                    optionTwoPenalty.AddEntityLoss(companion, EntityStatTypes.CurrentHealth,
                        ReferenceEquals(companion, chosenCompanion) ? 15 : 5);
                }
            }

            var optionTwo = new Option(optionTitle, optionResultText, optionTwoReward, optionTwoPenalty, EncounterType);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
