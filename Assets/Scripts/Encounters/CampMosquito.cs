using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;

namespace Assets.Scripts.Encounters
{
    public class CampMosquito : Encounter
    {
        public CampMosquito()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Camp Mosquito";
            Description = "The group happens upon an old campsite. During the night, they are attacked by swarms of thirsty mosquitoes and must leave before shutting an eye!";

            // Penalty = new Penalty();
            // Penalty.AddEntityLoss(TravelManager.Instance.Party.Derpus, EntityStatTypes.CurrentEnergy, 10);
            //
            // foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            // {
            //     Penalty.AddEntityLoss(companion, EntityStatTypes.CurrentEnergy, 10);
            // }
        }

        public override void Run()
        {
            Penalty = new Penalty();
            Penalty.AddEntityLoss(TravelManager.Instance.Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            {
                Penalty.AddEntityLoss(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var fullResultDescription = new List<string>();

            fullResultDescription.Add(Description + "\n");

            var penaltiesText = TravelManager.Instance.ApplyEncounterPenalty(Penalty);

            fullResultDescription.AddRange(penaltiesText);

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
