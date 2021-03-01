using System.Collections.Generic;

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
            var fullResultDescription = new List<string> { Description + "\n" };

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
