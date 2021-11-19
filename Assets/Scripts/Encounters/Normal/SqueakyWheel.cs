using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class SqueakyWheel : Encounter
    {
        public SqueakyWheel()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Squeaky Wheel Gets The Grease";
        }

        public override void Run()
        {
            Description = @"The group gets irritated because one of the wagon's wheels is squeaking. Who should try to repair it?";

            Options = new Dictionary<string, Option>();

            var travelManager = Object.FindObjectOfType<TravelManager>();
            var chosenCompanions = travelManager.Party.GetRandomCompanions(4);

            const int fixSuccess = 10;

            Reward optionReward;
            Penalty optionPenalty;

            foreach (var chosenCompanion in chosenCompanions)
            {
                if (Options.ContainsKey(chosenCompanion.FirstName()))
                {
                    continue;
                }

                optionReward = null;
                optionPenalty = null;

                var optionTitle = $"{chosenCompanion.FirstName()}";

                string optionResultText;

                var fixCheck = Dice.Roll($"{chosenCompanion.Attributes.Acumen - 1}d6");

                var wildRoll = GlobalHelper.RollWildDie();

                fixCheck += wildRoll;

                if (fixCheck > fixSuccess)
                {
                    optionResultText = "They figure out what was wrong and repair it!";

                    optionReward = new Reward();
                    optionReward.AddEntityGain(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

                    foreach (var companion in travelManager.Party.GetCompanions())
                    {
                        optionReward.AddEntityGain(companion, EntityStatTypes.CurrentMorale, 10);
                    }
                }
                else
                {
                    optionResultText = "They can't repair it and the squeaking continues to be annoying.";

                    optionPenalty = new Penalty();
                    optionPenalty.AddEntityLoss(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

                    foreach (var companion in travelManager.Party.GetCompanions())
                    {
                        optionPenalty.AddEntityLoss(companion, EntityStatTypes.CurrentMorale, 10);
                    }
                }

                var option = new Option(optionTitle, optionResultText, optionReward, optionPenalty, EncounterType);

                Options.Add(optionTitle, option);
            }

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
