using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class GenieInABottle : Encounter
    {
        public GenieInABottle()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Normal;
            Title = "Genie In a Bottle";
        }

        public override void Run()
        {
            var chosenCompanion = TravelManager.Instance.Party.GetRandomCompanion();

            Description =
                $"While taking a whizz {chosenCompanion.Name} notices a black bottle sticking out from under a bush. They pick it up and inspect it, but they can't quite make out the contents. Curiosity gets the best of {chosenCompanion.FirstName()} and they open the bottle. Black smoke erupts from the bottle and forms into a genie! \n\nMAKE A WISH!";

            Options = new Dictionary<string, Option>();

            var optionTitle = "I want to feast tonight!";
            var optionResultText =
                $"The genie snaps his fingers and {chosenCompanion.FirstName()}'s arms are overflowing with delicious food!";

            var reward = new Reward();

            var foodGained = Random.Range(15, 21);

            reward.AddPartyGain(PartySupplyTypes.Food, foodGained);

            var optionOne = new Option(optionTitle, optionResultText, reward, null, EncounterType);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Good health!";
            optionResultText = $"{chosenCompanion.FirstName()}'s wounds are completely healed! They feel like a million bucks!";

            reward = new Reward();

            reward.AddEntityGain(chosenCompanion, EntityStatTypes.CurrentHealth, chosenCompanion.Stats.MaxHealth);
            reward.AddEntityGain(chosenCompanion, EntityStatTypes.CurrentEnergy, chosenCompanion.Stats.MaxEnergy);

            var optionTwo = new Option(optionTitle, optionResultText, reward, null, EncounterType);

            Options.Add(optionTitle, optionTwo);

            optionTitle = "MAKE IT RAIN";
            optionResultText = $"The genie disappears in a flash! Gold coins rain down upon {chosenCompanion.FirstName()}'s skull much to their delight!";

            var goldAmount = Random.Range(20, 81);

            reward = new Reward();

            reward.AddPartyGain(PartySupplyTypes.Gold, goldAmount);

            var optionThree = new Option(optionTitle, optionResultText, reward, null, EncounterType);

            Options.Add(optionTitle, optionThree);

            SubscribeToOptionSelectedEvent();

            EventMediator.Instance.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
