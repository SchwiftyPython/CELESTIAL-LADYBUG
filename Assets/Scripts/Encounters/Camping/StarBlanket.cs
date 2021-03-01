using System.Collections.Generic;

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
        }

        public override void Run()
        {
            var fullResultDescription = new List<string> {Description + "\n"};

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
