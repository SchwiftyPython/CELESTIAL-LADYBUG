using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class TrueLove : Encounter
    {
        public TrueLove()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Camping;
            Title = "True Love";
            CountsAsDayTraveled = true;
        }
        public override void Run()
        {
            var chosen = Party.GetRandomCompanion();

            Description = $"You and the party get a free stay at an inn with some coupons. It's a pretty comfy place, but {chosen.FirstName()} has to deal with a drunk bard serenading to their lover outside their window! Eventually, the bard figures out he is at the wrong inn and leaves.";

            Reward = new Reward();

            Reward.AddEntityGain(Party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in Party.GetCompanions())
            {
                if (companion == chosen)
                {
                    Reward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 5);
                    continue;
                }

                Reward.AddEntityGain(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterReward(Reward);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
