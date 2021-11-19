using System.Collections.Generic;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters.Camping
{
    public class ArachnidArchery : Encounter
    {
        private const int MinSpiders = 5;
        private const int MaxSpiders = 7;

        public ArachnidArchery()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Camping;
            Title = "Arachnid Archery";
            CountsAsDayTraveled = true;
            ImageName = "archery";
        }

        public override void Run()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();

            var chosenCompanion = travelManager.Party.GetRandomCompanion();

            Description =
                $"The group finds an abandoned archery range with a surprising amount of arrows and even a few bows. The sun sets and they decide to camp here for the night. \n\n{chosenCompanion.FirstName()} looks outside and notices a horde of giant spiders headed straight towards the range!";

            Options = new Dictionary<string, Option>();

            var optionTitle = "Shoot arrows at the spiders";

            string optionResultText;

            var party = travelManager.Party;

            var partyEndurance = party.GetTotalPartyEndurance();
            var partyPhysique = party.GetTotalPartyPhysique();

            Reward optionOneReward = null;
            Penalty optionOnePenalty = null;

            if (partyEndurance < 8)
            {
                optionResultText =
                    "Shooting arrows uses a lot of energy. The group gets tired and the spiders overrun the range.";

                optionOnePenalty = new Penalty();

                foreach (var companion in party.GetCompanions())
                {
                    optionOnePenalty.AddEntityLoss(companion, EntityStatTypes.CurrentHealth, 5);
                }
            }
            else if (partyPhysique < 8)
            {
                optionResultText =
                    "They send arrows raining into the spiders, but they lack the strength to fire the arrows effectively. A few spiders are injured, but the group ends up having to fight them hand to hand.";

                optionOnePenalty = new Penalty();

                optionOnePenalty.AddEntityLoss(party.GetRandomCompanion(), EntityStatTypes.CurrentHealth, 5);

                if (party.GetCompanions().Count > 1)
                {
                    optionOnePenalty.AddEntityLoss(party.GetRandomCompanion(), EntityStatTypes.CurrentHealth, 5);
                }
            }
            else
            {
                optionResultText =
                    "Spider after spider falls dead as arrows slam into them from afar. They were barely a threat!";

                optionOneReward = new Reward();

                optionOneReward.AddEntityGain(party.Derpus, EntityStatTypes.CurrentMorale, 5);

                foreach (var companion in party.GetCompanions())
                {
                    optionOneReward.AddEntityGain(companion, EntityStatTypes.CurrentMorale, 5);
                }

                optionOneReward.AddEntityGain(party.GetCompanionWithLowestEndurance(), EntitySkillTypes.Endurance, 1);
            }

            var optionOne = new Option(optionTitle, optionResultText, optionOneReward, optionOnePenalty,
                EncounterType.Camping);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Escape!";

            optionResultText = "The party makes a run for it and doesn't stop til sun up.";

            var optionTwoPenalty = new Penalty();

            optionTwoPenalty.AddEntityLoss(party.Derpus, EntityStatTypes.CurrentEnergy, 10);

            foreach (var companion in party.GetCompanions())
            {
                optionTwoPenalty.AddEntityLoss(companion, EntityStatTypes.CurrentEnergy, 10);
            }

            var optionTwo = new Option(optionTitle, optionResultText, null, optionTwoPenalty, EncounterType.Camping);

            Options.Add(optionTitle, optionTwo);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
