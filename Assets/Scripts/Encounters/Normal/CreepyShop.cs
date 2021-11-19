using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class CreepyShop : Encounter
    {
        public CreepyShop()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Creepy Shop";
        }

        public override void Run()
        {
            Description = @"The group finds a run-down shop. Looks like there may be stuff inside, but it's hard to make out anything in the darkness. It gives everyone the creeps.";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Ignore the creepy building and keep moving";

            string optionResultText = "The group moves on and soon forgets about the whole incident.";

            var optionOne = new Option(optionTitle, optionResultText, EncounterType.Normal);

            Options.Add(optionTitle, optionOne);

            var travelManager = Object.FindObjectOfType<TravelManager>();

            optionTitle = "Send someone in to investigate";

            var chosenCompanion = travelManager.Party.GetRandomCompanion();

            Reward optionTwoReward = null;
            Penalty optionTwoPenalty = null;

            const int acumenSuccess = 15;

            var acumenRoll = Dice.Roll($"{chosenCompanion.Attributes.Acumen - 1}d6");

            var wildRoll = GlobalHelper.RollWildDie();

            acumenRoll += wildRoll;

            if (chosenCompanion.Attributes.Intellect > 5 && chosenCompanion.Attributes.Charisma < 3)
            {
                optionResultText =
                    $"They wander into the darkness mumbling and singing to themselves to distract from the creepiness. As they near the back of the store, the fear overcomes them. {chosenCompanion.FirstName()} sees a tiny spider and screams like a banshee!";

                optionTwoPenalty = new Penalty();

                optionTwoPenalty.AddEntityLoss(chosenCompanion, EntityStatTypes.CurrentMorale, 10);
            }
            else if (chosenCompanion.Attributes.Charisma > 5 && chosenCompanion.Attributes.Intellect < 3)
            {
                optionResultText = $"{chosenCompanion.FirstName()} doesn't notice anything creepy or dangerous in there.";

                optionTwoReward = new Reward();

                optionTwoReward.AddPartyGain(PartySupplyTypes.Food, Random.Range(7, 11));

                const int potionChance = 70;

                var roll = Dice.Roll("1d100");

                if (roll <= potionChance)
                {
                    optionTwoReward.AddPartyGain(PartySupplyTypes.HealthPotions, Random.Range(3, 8));
                }

                const int goldChance = 30;

                roll = Dice.Roll("1d100");

                if (roll <= goldChance)
                {
                    optionTwoReward.AddPartyGain(PartySupplyTypes.Gold, Random.Range(30, 61));
                }
            }
            else if (chosenCompanion.Attributes.Acumen > 5 || acumenRoll > acumenSuccess)
            {
                optionResultText = $"{chosenCompanion.FirstName()} keeps calm, grabs what they can find, and gets the heck out of there!";

                optionTwoReward = new Reward();

                optionTwoReward.AddPartyGain(PartySupplyTypes.Food, Random.Range(7, 11));

                const int potionChance = 70;

                var roll = Dice.Roll("1d100");

                if (roll <= potionChance)
                {
                    optionTwoReward.AddPartyGain(PartySupplyTypes.HealthPotions, Random.Range(3, 8));
                }

                const int goldChance = 30;

                roll = Dice.Roll("1d100");

                if (roll <= goldChance)
                {
                    optionTwoReward.AddPartyGain(PartySupplyTypes.Gold, Random.Range(30, 61));
                }
            }
            else
            {
                optionResultText =
                    $"{chosenCompanion.FirstName()} disappears into the darkness for a few moments before sprinting out of the shop claiming there was nothing worth taking!";

                optionTwoPenalty = new Penalty();

                optionTwoPenalty.AddEntityLoss(chosenCompanion, EntityStatTypes.CurrentMorale, 5);
            }

            var optionTwo = new Option(optionTitle, optionResultText, optionTwoReward, optionTwoPenalty, EncounterType);

            Options.Add(optionTitle, optionTwo);

            optionTitle = "Send the whole group in!";

            Penalty optionThreePenalty = null;

            Entity scaredCompanion = null;
            foreach (var companion in travelManager.Party.GetCompanions())
            {
                if (companion.Attributes.Acumen < 2)
                {
                    scaredCompanion = companion;
                    break;
                }
            }

            var optionThreeReward = new Reward();

            if (scaredCompanion != null)
            {
                optionResultText =
                    $"{scaredCompanion.FirstName()} attacks another party member after mistaking them for a ghost! The whole group panics and starts swinging their weapons wildly! Once the dust settles, the building is barely left standing.";

                optionThreePenalty = new Penalty();

                foreach (var companion in travelManager.Party.GetCompanions())
                {
                    optionThreePenalty.AddEntityLoss(companion, EntityStatTypes.CurrentHealth, 5);
                }

                optionThreeReward.AddPartyGain(PartySupplyTypes.Food, Random.Range(7, 11));

                const int potionChance = 70;

                var roll = Dice.Roll("1d100");

                if (roll <= potionChance)
                {
                    optionThreeReward.AddPartyGain(PartySupplyTypes.HealthPotions, Random.Range(3, 8));
                }

                const int goldChance = 30;

                roll = Dice.Roll("1d100");

                if (roll <= goldChance)
                {
                    optionThreeReward.AddPartyGain(PartySupplyTypes.Gold, Random.Range(30, 61));
                }
            }
            else
            {
                optionResultText = $"Everyone keeps their cool and grabs what they can find.";

                optionThreeReward.AddPartyGain(PartySupplyTypes.Food, Random.Range(7, 11));

                const int potionChance = 70;

                var roll = Dice.Roll("1d100");

                if (roll <= potionChance)
                {
                    optionThreeReward.AddPartyGain(PartySupplyTypes.HealthPotions, Random.Range(3, 8));
                }

                const int goldChance = 30;

                roll = Dice.Roll("1d100");

                if (roll <= goldChance)
                {
                    optionThreeReward.AddPartyGain(PartySupplyTypes.Gold, Random.Range(30, 61));
                }
            }

            var optionThree = new Option(optionTitle, optionResultText, optionThreeReward, optionThreePenalty,
                EncounterType.Normal);

            Options.Add(optionTitle, optionThree);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
