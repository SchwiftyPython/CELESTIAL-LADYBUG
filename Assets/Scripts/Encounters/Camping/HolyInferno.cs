using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class HolyInferno : Encounter
    {
        public HolyInferno()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Holy Inferno";
            Description = "The party finds an old, abandoned church on the trail. A dry place to sleep. Too dry. They awaken in the middle of the night and the building is on fire!";
            CountsAsDayTraveled = true;
        }

        public override void Run()
        {
            Options = new Dictionary<string, Option>();

            var optionTitle = "Look for a safe way out";

            const int escapeSuccess = 26;

            var travelManager = Object.FindObjectOfType<TravelManager>();
            var smartyPants = travelManager.Party.GetCompanionWithHighestIntellect();

            //todo diceroller here
            var intCheck = smartyPants.Attributes.Intellect + Random.Range(1, 21);

            Debug.Log("Holy Inferno safe way out check: ");
            Debug.Log($"Value Needed: {escapeSuccess}");
            Debug.Log(
                $"Rolled: {intCheck - smartyPants.Attributes.Intellect} + Intellect: {smartyPants.Attributes.Intellect} = Final Value {intCheck}");
            
            Penalty penalty = null;

            string optionResultText;

            if (intCheck > escapeSuccess)
            {
                optionResultText = $"{smartyPants.Name} thinks fast and finds a way out. No one is injured!";
            }
            else if (intCheck == escapeSuccess)
            {
                optionResultText = $"{smartyPants.Name} tries to find a safe exit, but wastes precious time making a decision. Everyone escapes, but they look a little crispy...";

                penalty = new Penalty();

                foreach (var companion in travelManager.Party.GetCompanions())
                {
                    penalty.AddEntityLoss(companion, EntityStatTypes.CurrentHealth, 10);
                }
            }
            else
            {
                optionResultText = $"{smartyPants.Name} tries to find a safe exit, but panics instead! Everyone runs screaming from the building!";

                penalty = new Penalty();
                penalty.AddEntityLoss(travelManager.Party.Derpus, EntityStatTypes.CurrentMorale, 10);

                foreach (var companion in travelManager.Party.GetCompanions())
                {
                    penalty.AddEntityLoss(companion, EntityStatTypes.CurrentMorale, 10);
                }
            }

            var optionOne = new Option(optionTitle, optionResultText, null, penalty, EncounterType);

            Options.Add(optionTitle, optionOne);

            optionTitle = "RUN!";

            var chosenCompanion = travelManager.Party.GetRandomCompanion();

            optionResultText = $"Everyone flees to safety in a panic. After ensuring everyone is unharmed, {chosenCompanion.Name} notices that their bag was left behind.";

            penalty = new Penalty();

            var foodLost = travelManager.Party.Food / 4;

            penalty.AddPartyLoss(PartySupplyTypes.Food, foodLost);

            var goldLost = travelManager.Party.Gold / 4;

            penalty.AddPartyLoss(PartySupplyTypes.Gold, goldLost);

            var potionsLost = travelManager.Party.HealthPotions / 4;

            penalty.AddPartyLoss(PartySupplyTypes.HealthPotions, potionsLost);

            var optionTwo = new Option(optionTitle, optionResultText, null, penalty, EncounterType);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
