using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class ImpromptuParade : Encounter
    {
        public ImpromptuParade()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Normal;
            Title = "Impromptu Parade";
            Description = "The wagon is rolling through a town. They're about halfway through when someone shouts \"THAT'S THEM!\"\n\nSuddenly fireworks are going off and people have filled the street in celebration! They throw money, flowers, and food everywhere until the party reaches the end of the town.\n\n\"THANK YOU FOR SAVING OUR TOWN!\"\n\nEveryone was able to grab some food and coin so no one tries to say otherwise. A short distance later, the party passes by another wagon outfitted with streamers accompanied by some fellows in festive clothing and armor.";
        }

        public override void Run()
        {
            Reward = new Reward();

            Reward.AddPartyGain(PartySupplyTypes.Gold, 80);
            Reward.AddPartyGain(PartySupplyTypes.Food, 9);
            Reward.EveryoneGain(Party, EntityStatTypes.CurrentMorale, 20);

            //todo Derpus is wearing a lei for the rest of the adventure

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.ApplyEncounterReward(Reward);

            var fullResultDescription = new List<string> { Description + "\n" };

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EncounterResult, this, fullResultDescription);
        }
    }
}
