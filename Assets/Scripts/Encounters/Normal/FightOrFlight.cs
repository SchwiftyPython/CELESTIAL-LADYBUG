﻿using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class FightOrFlight : Encounter
    {
        public FightOrFlight()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Fight or Flight";
            ImageName = "wall";
        }

        //todo refactor
        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();
            var chosenCompanion = travelManager.Party.GetRandomCompanion();

            Description =
                $"While scouting a town, {chosenCompanion.Name} decides the caravan would not be welcome here. Before they can leave, a guard sees through their disguise! {chosenCompanion.FirstName()} is cornered in an alley with large crates and a tall fence.";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Scale The Fence";

            string optionResultText;

            Reward optionOneReward = null;
            Penalty optionOnePenalty = null;

            const int scaleSuccess = 10;

            var agilityCheck = Dice.Roll($"{chosenCompanion.Attributes.Agility - 1}d6");

            var wildRoll = GlobalHelper.RollWildDie();

            agilityCheck += wildRoll;

            Debug.Log($"Value Needed: {scaleSuccess}");
            Debug.Log(
                $"Rolled: {agilityCheck - chosenCompanion.Attributes.Agility} + Agility: {chosenCompanion.Attributes.Agility} = Final Value {agilityCheck}");

            if (agilityCheck > scaleSuccess)
            {
                optionResultText = $"{chosenCompanion.FirstName()} runs up one of the crates and flips over the fence!";

                optionOneReward = new Reward();
                optionOneReward.AddEntityGain(chosenCompanion, EntityStatTypes.CurrentMorale, 30);
            }
            else if (agilityCheck == scaleSuccess)
            {
                optionResultText = $"{chosenCompanion.FirstName()} barely manages to escape over the fence unharmed!";
            }
            else
            {
                optionResultText =
                    $"{chosenCompanion.FirstName()} struggles to climb the fence. One of the guards deals a blow to his leg before he makes it over!";

                optionOnePenalty = new Penalty();
                optionOnePenalty.AddEntityLoss(chosenCompanion, EntityStatTypes.CurrentHealth, 15);
            }

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, optionOnePenalty, EncounterType);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Throw Crates";

            Reward optionTwoReward = null;
            Penalty optionTwoPenalty = null;

            const int throwSuccess = 20;

            //todo diceroller here
            var physiqueCheck = chosenCompanion.Attributes.Physique + Random.Range(1, 21);

            Debug.Log($"Value Needed: {throwSuccess}");
            Debug.Log(
                $"Rolled: {physiqueCheck - chosenCompanion.Attributes.Physique} + Physique: {chosenCompanion.Attributes.Physique} = Final Value {physiqueCheck}");

            if (physiqueCheck >= throwSuccess)
            {
                optionResultText =
                    $"{chosenCompanion.FirstName()} knocks several guards to the ground with a well placed throw and escapes!";

                optionTwoReward = new Reward();
                optionTwoReward.AddEntityGain(chosenCompanion, EntityStatTypes.CurrentMorale, 30);
            }
            else
            {
                optionResultText =
                    $"{chosenCompanion.FirstName()} manages to hit a guard or two with the crates, but has to fight off the rest!";

                optionTwoPenalty = new Penalty();
                optionTwoPenalty.AddEntityLoss(chosenCompanion, EntityStatTypes.CurrentHealth, 30);
            }

            var optionTwo = new Option(optionTitle, optionResultText, optionTwoReward, optionTwoPenalty, EncounterType);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
