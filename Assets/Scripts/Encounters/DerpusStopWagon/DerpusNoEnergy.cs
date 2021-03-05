using System.Collections.Generic;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters.DerpusStopWagon
{
    public class DerpusNoEnergy : Encounter
    {
        public DerpusNoEnergy()
        {
            EncounterType = EncounterType.Camping;
            Title = "Derpus Tired";
            Description =
                "The wagon slowly rolls to a stop. Derpus is too tired and has decided to nap the rest of the day.";
            CountsAsDayTraveled = false;
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
