using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class ExtremeFoodChallenge : Encounter
    {
        private readonly List<string> _challenges = new List<string>
        {
            "The Deadliest Catch",
            "5 Foot Megabeast Pizza",
            "Get Clucked Noob",
            "The Dragon"
        };

        public ExtremeFoodChallenge()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "EXTREME FOOD CHALLENGE";
        }

        public override void Run()
        {
            var challenge = _challenges[Random.Range(0, _challenges.Count)];

            Description =
                "The party is passing through a town and sees an advertisement for an EXTREME FOOD CHALLENGE outside of an inn:\n\n";

            switch (challenge)
            {
                case "The Deadliest Catch":
                    Description +=
                        "The Deadliest Catch! One hour to scarf down 40 fish filets, a bucket of slaw, and 3 gallons of beans!";
                    break;
                case "5 Foot Megabeast Pizza":
                    Description +=
                        "The 5 Foot Megabeast Pizza Challenge! One hour to finish a 5 foot, 22 pound pizza smothered in meat!";
                    break;
                case "Get Clucked Noob":
                    Description +=
                        "The 'Get Clucked, Noob!' Challenge! Eat 32 of our hottest wings with no other food or beverage in under 10 minutes!";
                    break;
                case "The Dragon":
                    Description += "The Dragon Chili Challenge! Finish a drum of our infamous Dragon Chili in under an hour!";
                    break;
            }

            Description += $"\n\nWho will take the challenge?";

            var travelManager = Object.FindObjectOfType<TravelManager>();

            var challengers = travelManager.Party.GetRandomCompanions(4);

            Options = new Dictionary<string, Option>();

            foreach (var challenger in challengers)
            {
                string optionResultText = null;

                var optionReward = new Reward();

                switch (challenge)
                {
                    case "The Deadliest Catch":
                        optionResultText = $"{challenger.FirstName()} SHOVELS fish into their cakehole with one hand and slaw and beans with the other! With a loud belch they crash to the floor victorious!";
                        optionReward.AddEntityGain(challenger, EntityAttributeTypes.Intellect, 1);
                        optionReward.AddEntityGain(challenger, EntitySkillTypes.Endurance, 1);
                        break;
                    case "5 Foot Megabeast Pizza":
                        optionResultText = $"{challenger.FirstName()} ROLLS the pizza into a burrito and shows no mercy! They briefly enter a sweaty catatonic state, but snap out of it with a thunderous burp!";
                        optionReward.AddEntityGain(challenger, EntityAttributeTypes.Physique, 1);
                        optionReward.AddEntityGain(challenger, EntitySkillTypes.Endurance, 1);
                        break;
                    case "Get Clucked Noob":
                        optionResultText = $"{challenger.FirstName()} VACUUMS the meat off the bones in seconds! Their skin turns BRIGHT RED and enter a sweaty catatonic state for the better part of an hour.";
                        optionReward.AddEntityGain(challenger, EntitySkillTypes.Survival, 1);
                        optionReward.AddEntityGain(challenger, EntitySkillTypes.Endurance, 1);
                        break;
                    case "The Dragon":
                        optionResultText = $"{challenger.FirstName()} CHUGS the chili! Their skin turns BRIGHT RED and they can't communicate anything through bouts of sweating and spice induced hallucinations. They recover after some time and declare to everyone that they have retired from spicy foods.";
                        optionReward.AddEntityGain(challenger, EntitySkillTypes.Survival, 1);
                        optionReward.AddEntityGain(challenger, EntitySkillTypes.Endurance, 1);
                        break;
                }

                var option = new Option(challenger.Name, optionResultText, optionReward, null,
                    EncounterType.Normal);

                Options.Add(challenger.Name, option);
            }

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
