using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class HowRude : Encounter
    {
        public HowRude()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "How Rude!";
        }

        public override void Run()
        {
            var entityStore = Object.FindObjectOfType<EntityPrefabStore>();

            var rudeFella = entityStore.GetRandomCompanion();  //todo give drunk trait

            var travelManager = Object.FindObjectOfType<TravelManager>();

            if (travelManager.Party.IsFull())
            {
                Description = $"The party passes by a {rudeFella.EntityClass.ToString().ToLower()} face down on the ground. They stir and wobble to their knees. \n\n\"Oi! Ye wagon is a hunk of junk!\"\n\nDerpus continues to tug the wagon along with tears in his eyes.";

                Penalty = new Penalty();

                Penalty.AddEntityLoss(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 5);

                travelManager.ApplyEncounterPenalty(Penalty);
            }
            else
            {
                Description = $"The party passes by a {rudeFella.EntityClass.ToString().ToLower()} face down on the ground. Later, they make a stop to rest and notice the {rudeFella.EntityClass.ToString().ToLower()} clinging to the bottom of the wagon! \n\n\"Haha! I'm comin' with ya!\"";

                Reward = new Reward();

                Reward.AddToParty(rudeFella);

                travelManager.ApplyEncounterReward(Reward);
            }

            var fullResultDescription = new List<string> { Description + "\n" };

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
