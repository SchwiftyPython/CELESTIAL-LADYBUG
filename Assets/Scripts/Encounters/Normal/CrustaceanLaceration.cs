using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class CrustaceanLaceration : Encounter
    {
        //todo trigger blurb about not messing with crabs

        public CrustaceanLaceration()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Crustacean Laceration";
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            var chosenCompanion = travelManager.Party.GetRandomCompanion();

            Description = $"{chosenCompanion.FirstName()} gets cut pretty badly after challenging a crab to a knife fight.";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Ignore it";

            var optionResultText = $"{chosenCompanion.FirstName()} insists that they don't need any help and proudly bleeds everywhere.";

            var optionOnePenalty = new Penalty();

            optionOnePenalty.AddEntityLoss(chosenCompanion, EntityStatTypes.CurrentHealth, 5);
            optionOnePenalty.AddEntityLoss(chosenCompanion, EntityAttributeTypes.Physique, 1);

            var optionOne = new Option(optionTitle, optionResultText, null, optionOnePenalty, EncounterType.Normal);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Heal the wound";

            var medicalCompanion = travelManager.Party.GetCompanionWithHighestHealing();

            var healRoll = Dice.Roll($"{medicalCompanion.Skills.Healing - 1}d6");

            var wildRoll = GlobalHelper.RollWildDie();

            healRoll += wildRoll;

            const int healSuccess = 15;

            Reward optionTwoReward = null;
            Penalty optionTwoPenalty;

            if (medicalCompanion.Skills.Healing > 5 || healRoll > healSuccess)
            {
                if (travelManager.Party.HealthPotions > 0)
                {
                    optionResultText =
                        $"{medicalCompanion.FirstName()} is able to treat the wound and it heals properly.";

                    optionTwoReward = new Reward();

                    optionTwoReward.AddEntityGain(medicalCompanion, EntitySkillTypes.Healing, 1);

                    optionTwoPenalty = new Penalty();

                    optionTwoPenalty.AddPartyLoss(PartySupplyTypes.HealthPotions, 1);
                }
                else
                {
                    optionResultText =
                        $"{medicalCompanion.FirstName()} does the best they can with no health potions. The wound ends up smelling funny later on.";

                    optionTwoPenalty = new Penalty();

                    optionTwoPenalty.AddEntityLoss(chosenCompanion, EntityStatTypes.CurrentMorale, 5);
                }
            }
            else
            {
                optionResultText =
                    $"{medicalCompanion.FirstName()} tries to help, but makes the wound much worse. The bleeding stops, but {chosenCompanion.FirstName()} feels weaker.";

                optionTwoPenalty = new Penalty();

                optionTwoPenalty.AddEntityLoss(chosenCompanion, EntityStatTypes.CurrentHealth, 5);
                optionTwoPenalty.AddEntityLoss(chosenCompanion, EntityAttributeTypes.Physique, 1);
            }

            var optionTwo = new Option(optionTitle, optionResultText, optionTwoReward, optionTwoPenalty,
                EncounterType.Normal);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
