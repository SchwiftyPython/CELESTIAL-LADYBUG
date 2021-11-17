using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Companions;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Combat
{
    public class NervousBandits : Encounter
    {
        private const int MinBandits = 2;
        private const int MaxBandits = 4;

        public NervousBandits()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Combat;
            Title = "Nervous Bandits";
        }

        public override void Run()
        {
            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            Description = $"{numBandits} bandits have blocked the trail. ";

            const int foodThreshold = 6;

            Description += "Their leader steps forward and shakily demands that you turn over ";

            Penalty = new Penalty();

            if (Party.Food > foodThreshold && Party.Gold > 4 && Party.HealthPotions > 4)
            {
                var numGold = Party.Gold / 4;

                Description += $"{numGold} gold, ";
                
                var numPotions = Party.HealthPotions / 4;

                Description += $"{numPotions} potion";

                if (numPotions > 1)
                {
                    Description += $"s";
                }

                Description += ", ";

                Description += "and the rest of your food!";

                Penalty.AddPartyLoss(PartySupplyTypes.Gold, Party.Gold / 4);
                Penalty.AddPartyLoss(PartySupplyTypes.HealthPotions, Party.HealthPotions / 4);
                Penalty.AddPartyLoss(PartySupplyTypes.Food, Party.Food);

            }
            else
            {
                Description += "half of all your supplies!";

                if (Party.Gold > 1)
                {
                    Penalty.AddPartyLoss(PartySupplyTypes.Gold, Party.Gold / 2);
                }

                if (Party.HealthPotions > 1)
                {
                    Penalty.AddPartyLoss(PartySupplyTypes.HealthPotions, Party.HealthPotions / 2);
                }

                if (Party.Food > 1)
                {
                    Penalty.AddPartyLoss(PartySupplyTypes.Food, Party.Food / 2);
                }
            }

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

            var optionOnePenalty = Penalty;

            Penalty = null;

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
