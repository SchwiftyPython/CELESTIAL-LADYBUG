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

            var entityLosses = new Dictionary<Entity, KeyValuePair<object, int>>
            {
                {TravelManager.Instance.Party.Derpus, new KeyValuePair<object, int>("energy", 10)}
            };

            foreach (var companion in TravelManager.Instance.Party.GetCompanions())
            {
                entityLosses.Add(companion, new KeyValuePair<object, int>("energy", 10));
            }

            Penalty = new Penalty(entityLosses);
        }

        public override void Run()
        {
            var fullResultDescription = new List<string>();

            fullResultDescription.Add(Description + "\n");

            var penaltiesText = TravelManager.Instance.ApplyEncounterPenalty(Penalty);

            fullResultDescription.AddRange(penaltiesText);

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
