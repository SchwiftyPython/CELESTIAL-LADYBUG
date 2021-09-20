using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class NoFirewood : Encounter
    {
        public NoFirewood()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "No Firewood";
            Description = $"It is going to be a chilly night and you've run out of firewood. Someone can chop some, but it's tiring work.";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            var volunteers = Party.GetRandomCompanions(3);

            string optionResultText;
            foreach (var volunteer in volunteers)
            {
                optionResultText = $"The fire really hits the spot!";

                var optionReward = new Reward();

                optionReward.AddEntityGain(volunteer, EntityAttributeTypes.Physique, 1);

                optionReward.EveryoneGain(Party, EntityStatTypes.CurrentMorale, 5);

                optionReward.AddEntityGain(Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

                foreach (var companion in Party.GetCompanions())
                {
                    if (companion == volunteer)
                    {
                        continue;
                    }

                    optionReward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
                }

                var optionPenalty = new Penalty();

                optionPenalty.AddEntityLoss(volunteer, EntityStatTypes.CurrentEnergy, 20);

                var option = new Option(volunteer.Name, optionResultText, null, optionPenalty, EncounterType.Camping);

                Options.Add(volunteer.Name, option);
            }

            var optionTitle = "Don't bother chopping any";

            var littleSpoon = Party.GetRandomCompanion();

            optionResultText = $"Everyone is curled up into a ball trying to stay warm. {littleSpoon.FirstName()} wakes up to Derpus spooning them!";

            var optionFourReward = new Reward();

            optionFourReward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 10);

            var optionFourPenalty = new Penalty();

            optionFourPenalty.AddEntityLoss(littleSpoon, EntityStatTypes.CurrentMorale, 5);

            var optionFour = new Option(optionTitle, optionResultText, optionFourReward, optionFourPenalty,
                EncounterType.Camping);

            Options.Add(optionTitle, optionFour);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
