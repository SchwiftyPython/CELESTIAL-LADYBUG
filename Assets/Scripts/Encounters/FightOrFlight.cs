using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters
{
    public class FightOrFlight : Encounter
    {
        public FightOrFlight()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Fight or Flight";
        }

        //todo refactor
        public override void Run()
        {
            var chosenCompanion = TravelManager.Instance.Party.GetRandomCompanion();

            Description =
                $"While scouting a town, {chosenCompanion.Name} decides the caravan would not be welcome here. Before they can leave, a guard sees through their disguise! {chosenCompanion.FirstName()} is cornered in an alley with large crates and a tall fence.";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Scale The Fence";

            string optionResultText;

            Reward optionOneReward = null;
            Penalty optionOnePenalty = null;

            const int scaleSuccess = 10;

            //todo diceroller here
            var speedCheck = chosenCompanion.Attributes.Speed + Random.Range(1, 21);

            Debug.Log($"Value Needed: {scaleSuccess}");
            Debug.Log(
                $"Rolled: {speedCheck - chosenCompanion.Attributes.Speed} + Speed: {chosenCompanion.Attributes.Speed} = Final Value {speedCheck}");

            if (speedCheck > scaleSuccess)
            {
                optionResultText = $"{chosenCompanion.FirstName()} runs up one of the crates and flips over the fence!";

                var entityGains = new Dictionary<Entity, KeyValuePair<object, int>>
                {
                    {chosenCompanion, new KeyValuePair<object, int>("morale", 30)}
                };

                optionOneReward = new Reward(entityGains);
            }
            else if (speedCheck == scaleSuccess)
            {
                optionResultText = $"{chosenCompanion.FirstName()} barely manages to escape over the fence unharmed!";
            }
            else
            {
                optionResultText =
                    $"{chosenCompanion.FirstName()} struggles to climb the fence. One of the guards deals a blow to his leg before he makes it over!";

                var entityLosses = new Dictionary<Entity, KeyValuePair<object, int>>
                {
                    {chosenCompanion, new KeyValuePair<object, int>("health", 15)}
                };

                optionOnePenalty = new Penalty(entityLosses);
            }

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, optionOnePenalty);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Throw Crates";

            Reward optionTwoReward = null;
            Penalty optionTwoPenalty = null;

            const int throwSuccess = 20;

            //todo diceroller here
            var mightCheck = chosenCompanion.Attributes.Might + Random.Range(1, 21);

            Debug.Log($"Value Needed: {throwSuccess}");
            Debug.Log(
                $"Rolled: {mightCheck - chosenCompanion.Attributes.Might} + Might: {chosenCompanion.Attributes.Might} = Final Value {mightCheck}");

            if (mightCheck >= throwSuccess)
            {
                optionResultText =
                    $"{chosenCompanion.FirstName()} knocks several guards to the ground with a well placed throw and escapes!";

                var entityGains = new Dictionary<Entity, KeyValuePair<object, int>>
                {
                    {chosenCompanion, new KeyValuePair<object, int>("morale", 30)}
                };

                optionTwoReward = new Reward(entityGains);
            }
            else
            {
                optionResultText =
                    $"{chosenCompanion.FirstName()} manages to hit a guard or two with the crates, but has to fight off the rest!";

                var entityLosses = new Dictionary<Entity, KeyValuePair<object, int>>
                {
                    {chosenCompanion, new KeyValuePair<object, int>("health", 30)}
                };

                optionTwoPenalty = new Penalty(entityLosses);
            }

            var optionTwo = new Option(optionTitle, optionResultText, optionTwoReward, optionTwoPenalty);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            EventMediator.Instance.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
