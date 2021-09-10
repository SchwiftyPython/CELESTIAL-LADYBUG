using System.Collections.Generic;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class LendingAHand : Encounter
    {
        public LendingAHand() //todo continuity -- leave him there and he could pop up later for revenge!
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Normal;
            Title = "Lending a Hand";
        }

        public override void Run()
        {
            var entityStore = Object.FindObjectOfType<EntityPrefabStore>();

            var trappedFella = entityStore.GetRandomCompanion();

            var eClass = trappedFella.EntityClass.ToString().ToLower();

            var travelManager = Object.FindObjectOfType<TravelManager>();

            Description =
                $"While scouting a potential campsite, the party finds a {eClass} trapped under a fallen tree. Evidently, woodcutting is not their strong suit. A bag of supplies rests on the ground just out of the {eClass}'s reach. \n\n\"A little help?\"";

            Options = new Dictionary<string, Option>();

            var optionTitle = $"Rescue the {eClass}";
            string optionResultText;

            Reward optionOneReward;

            const int joinChance = 16;

            var joinRoll = Dice.Roll("1d100");

            if (!travelManager.Party.IsFull() && joinRoll <= joinChance)
            {
                optionResultText =
                    $"Everyone helps lift the tree so the {eClass} can escape. They spring to their feet and shake everyone's hand! \n\n\"I owe you a life debt! Name's {trappedFella.Name}! I'm comin' with!\"";

                optionOneReward = new Reward();

                optionOneReward.AddToParty(trappedFella);

                optionOneReward.AddPartyGain(PartySupplyTypes.HealthPotions, Random.Range(2, 5));
                optionOneReward.AddPartyGain(PartySupplyTypes.Food, Random.Range(1, 7));
                optionOneReward.AddPartyGain(PartySupplyTypes.Gold, Random.Range(5, 16));
            }
            else
            {
                optionResultText =
                    $"Everyone helps lift the tree so the {eClass} can escape. They're not able to give us a reward, but they promise to pay it forward.";

                optionOneReward = new Reward();

                optionOneReward.EveryoneGain(travelManager.Party, EntityStatTypes.CurrentMorale, 5);
            }

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, null, EncounterType.Normal);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Steal his supplies";

            var chosen = travelManager.Party.GetRandomCompanion();

            optionResultText = $"{chosen.FirstName()} grabs the bag of supplies and leaves the {eClass} to their fate.";

            var optionTwoReward = new Reward();

            optionTwoReward.AddPartyGain(PartySupplyTypes.HealthPotions, Random.Range(2, 5));
            optionTwoReward.AddPartyGain(PartySupplyTypes.Food, Random.Range(1, 7));
            optionTwoReward.AddPartyGain(PartySupplyTypes.Gold, Random.Range(5, 16));

            var optionTwo = new Option(optionTitle, optionResultText, optionTwoReward, null, EncounterType.Normal);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
