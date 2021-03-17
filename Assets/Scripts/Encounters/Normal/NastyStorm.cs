using System.Collections.Generic;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters.Normal
{
    public class NastyStorm : Encounter
    {
        public NastyStorm()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Nasty Storm";
            Description = "The group gets caught in a sudden downpour! Thunder crashes around them as they look for a place to take shelter, but there is none! With no other options, they are forced to keep moving through the elemental onslaught.";
        }

        public override void Run()
        {
            Penalty = new Penalty();
            Penalty.AddEntityLoss(TravelManager.Instance.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

            foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            {
                Penalty.AddEntityLoss(companion, EntityStatTypes.CurrentMorale, 10);
            }

            var fullResultDescription = new List<string> {Description + "\n"};

            var penaltiesText = TravelManager.Instance.ApplyEncounterPenalty(Penalty);

            fullResultDescription.AddRange(penaltiesText);

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
