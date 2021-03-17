using System.Collections.Generic;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters.DerpusStopWagon
{
    public class DerpusNoMorale : Encounter
    {
        public DerpusNoMorale()
        {
            EncounterType = EncounterType.Camping;
            Title = "Derpus Sad";
            Description =
                "The wagon slowly rolls to a stop. Derpus droops to a sad squat and refuses to move for the rest of the day.";
            CountsAsDayTraveled = false;
        }

        public override void Run()
        {
            var derpus = TravelManager.Instance.Party.Derpus;

            Reward = new Reward();

            Reward.AddEntityGain(derpus, EntityStatTypes.CurrentEnergy, derpus.Stats.MaxMorale / 2);

            Reward.AddEntityGain(derpus, EntityStatTypes.CurrentEnergy, 10);

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
