using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class MysteriousSack : Encounter
    {
        public MysteriousSack()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Sack of Mystery";
            Description = "The wagon comes to a stop. Derpus points to a lone burlap sack in the middle of the trail.";
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            var optionTitle = "Open it";

            var optionResultText = "Derpus snatches it up and takes a look inside. There's a lot of junk, but there were a couple health potions in there!";

            var optionOneReward = new Reward();

            optionOneReward.AddPartyGain(PartySupplyTypes.HealthPotions, 3);

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, null, EncounterType.Normal);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Carefully go around it";

            optionResultText = "Everyone gives the sack a wide berth and leaves it behind untouched.";

            var optionTwo = new Option(optionTitle, optionResultText, null, null, EncounterType.Normal);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
