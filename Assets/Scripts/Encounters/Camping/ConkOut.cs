using System.Collections.Generic;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters.Camping
{
    public class ConkOut : Encounter
    {
        public ConkOut()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "Conk out";
            Description = "After a particularly long day on the trail, the weary crew sets up camp and falls asleep instantly.";
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
