using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

//todo refactor
namespace Assets.Scripts.Encounters.Normal
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
            
            var reward = new Reward();
            reward.AddPartyGain(PartySupplyTypes.Food, 10);

            var penalty = new Penalty();

            var travelManager = Object.FindObjectOfType<TravelManager>();
            penalty.AddEntityLoss(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

            var optionOne = new Option(optionTitle, optionResultText, reward, penalty, EncounterType);

            Options.Add(optionTitle, optionOne);

            optionTitle = "We don't need any learnin'";
            optionResultText =
                "You decide to pass it by. You swear you catch Derpus staring at it as it fades out of sight.";

            var optionTwo = new Option(optionTitle, optionResultText, EncounterType);

            Options.Add(optionTitle, optionTwo);
        }

        public override void Run()
        {
            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
