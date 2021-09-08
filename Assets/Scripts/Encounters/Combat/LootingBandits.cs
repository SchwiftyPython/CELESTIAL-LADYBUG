using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Companions;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Combat
{
    public class LootingBandits : Encounter
    {
        private const int MinBandits = 3;
        private const int MaxBandits = 5;

        public LootingBandits()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Combat;
            Title = "Looting Bandits";
        }

        public override void Run()
        {
            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            Description = $"{numBandits} bandits have blocked the trail. Their leader steps forward and demands you turn over all your supplies.";

            var bandits = new List<Entity>();

            for (var i = 0; i < numBandits; i++)
            {
                var banditIndex = Dice.Roll("1d2");

                Entity bandit;

                if (banditIndex == 1)
                {
                    bandit = new ManAtArms(Race.RaceType.Human, false);
                }
                else
                {
                    bandit = new Crossbowman(Race.RaceType.Human, false);
                }

                bandits.Add(bandit);
            }

            Options = new Dictionary<string, Option>();

            var optionTitle = "Give up the supplies.";

            string optionResultText = "The group stands aside and watches as the bandits make off with their supplies.";

            var optionOnePenalty = new Penalty();

            var travelManager = Object.FindObjectOfType<TravelManager>();

            optionOnePenalty.AddPartyLoss(PartySupplyTypes.Gold, travelManager.Party.Gold);
            optionOnePenalty.AddPartyLoss(PartySupplyTypes.HealthPotions, travelManager.Party.HealthPotions);
            optionOnePenalty.AddPartyLoss(PartySupplyTypes.Food, travelManager.Party.Food);

            var optionOne = new Option(optionTitle, optionResultText, null, optionOnePenalty, EncounterType.Normal);

            Options.Add(optionTitle, optionOne);

            optionTitle = "Fight!";

            optionResultText = "Those supplies are essential to the group's survival. They must fight!";

            var fightOption = new FightCombatOption(optionTitle, optionResultText, bandits);

            Options.Add(optionTitle, fightOption);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
