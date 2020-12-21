using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;

//todo refactor
namespace Assets.Scripts.Encounters
{
    public class StayInSchool : Encounter
    {
        public StayInSchool()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Stay In School Kids";
            Description = @"The group spots a schoolhouse off the trail.";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Send Derpus to investigate";
            var optionResultText =
                "Derpus SPRINTS towards the schoolhouse, the wagon bouncing behind him. He always wanted to learn to read! Alas, the teacher explains to him that they don't have any room for him. The best she can do is offer some food if he agrees to be a guest speaker. Derpus agrees and speaks to the children about the importance of staying in school using himself as an example.";
            
            var partyGains = new Dictionary<object, int>
            {
                {"food", 10}
            };
            var reward = new Reward(partyGains);

            //todo encapsulate this in an EntityLoss class or in the Penalty class
            var entityLosses = new Dictionary<Entity, KeyValuePair<object, int>>
            {
                {TravelManager.Instance.Party.Derpus, new KeyValuePair<object, int>("morale", 10)}
            };

            var penalty = new Penalty(entityLosses);

            var optionOne = new Option(optionTitle, optionResultText, reward, penalty);

            Options.Add(optionTitle, optionOne);

            optionTitle = "We don't need any learnin'";
            optionResultText =
                "You decide to pass it by. You swear you catch Derpus staring at it as it fades out of sight.";

            var optionTwo = new Option(optionTitle, optionResultText);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();
        }

        public override void Run(Option selectedOption)
        {
            if (selectedOption == null)
            {
                Debug.Log("No option selected for Stay In School Encounter!");
                throw new ArgumentNullException(nameof(selectedOption));
            }

            if (selectedOption.HasReward())
            {
                TravelManager.Instance.ApplyEncounterReward(selectedOption.Reward);
            }


            //todo apply rewards
            //todo append rewards to description
            //todo apply penalties
            //todo append penalties to description

            //todo display result description
        }
    }
}
