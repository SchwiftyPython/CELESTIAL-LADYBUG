using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;

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

            //todo refactor encapsulate this in the Penalty class -- AddEntityLosses(List<Entity> companions, string targetStat, int value)
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
        }

        public override void Run()
        {
            SubscribeToOptionSelectedEvent();

            EventMediator.Instance.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
