using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class WelcomeToThePit : Encounter
    {
        public WelcomeToThePit()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "Music Festival Mishaps";
            CountsAsDayTraveled = true;
            //todo not last biome BiomeTypes = new List<BiomeType>();
        }

        public override void Run()
        {
            var victim = Party.GetRandomCompanion();

            Description = $"It's getting close to sundown when the sound of music carries through the air. A short while later, the party happens upon a music festival! Everyone decides this would be as good a spot to camp as any and parks the wagon out of the way.\n\n{victim.FirstName()} wanders towards a stage, but starts getting pushed around when the lute player is seven minutes into a solo! They try to get away, but go down for the count when they are hit in the head by a dwarf someone tossed!\n\nA festival goer stands over them and screams in their face \"WELCOME TO THE PIT!\"";

            Reward = new Reward();

            if (victim.Stats.CurrentHealth > 5)
            {
                Description += $"\n\n{victim.FirstName()} wakes in the morning with a horrible headache.";

                Reward.EveryoneGain(Party, EntityStatTypes.CurrentEnergy, 10);
            }
            else
            {
                Reward.AddEntityGain(Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

                foreach (var companion in Party.GetCompanions())
                {
                    if (companion == victim)
                    {
                        continue;
                    }

                    Reward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
                }
            }

            Penalty = new Penalty();

            Penalty.AddEntityLoss(victim, EntityStatTypes.CurrentHealth, 5);

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterReward(Reward);
            travelManager.ApplyEncounterPenalty(Penalty);

            var fullResultDescription = new List<string> { Description + "\n" };

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
