using System.Collections.Generic;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters.Camping
{
    public class StarBlanket : Encounter
    {
        public StarBlanket()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Star Blanket";
            Description = "The party decides to setup camp and have a decent meal around the campfire. Nothing of note takes place and they spend a peaceful night sleeping under the stars.";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            Reward = new Reward();

            Reward.AddEntityGain(TravelManager.Instance.Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            {
                Reward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            var rewardsText = TravelManager.Instance.ApplyEncounterReward(Reward);

            fullResultDescription.AddRange(rewardsText);

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
