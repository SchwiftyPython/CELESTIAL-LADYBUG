﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Necromancer;
using Assets.Scripts.Entities.Special;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Normal
{
    public class GhostFart : Encounter
    {
        public GhostFart()
        {
            Rarity = Rarity.Rare;
            EncounterType = EncounterType.Normal;
            Title = "Ghost Fart";
        }
    
        public override void Run()
        {
            Description = $"It's a narrow part of the trail and everyone has to walk in a tight group when someone farts!\n\n\"Who dun did it?\"";

            Options = new Dictionary<string, Option>();

            var farters = Party.GetRandomCompanions(3);

            string optionResultText;
            foreach (var farter in farters)
            {
                optionResultText = $"{farter.FirstName()} is the culprit!\n\nThe stench sticks to them like glue!\n\n\"Maybe check your drawers...\"";

                var optionPenalty = new Penalty();

                optionPenalty.AddEntityLoss(farter, EntityStatTypes.CurrentMorale, 10);

                var option = new Option(farter.Name, optionResultText, null, optionPenalty, EncounterType.Normal);

                Options.Add(farter.Name, option);
            }

            var optionTitle = "Burrito Ghost";

            var battleChance = 100;

            optionResultText = $"{farters.First().FirstName()} scoffs.\n\n\"Ain't no such thing as ghost wandering around, farting, and moaning 'burrito'!\"";

            var roll = Dice.Roll("1d100");

            Option optionFour;

            if (roll < battleChance)
            {
                optionResultText += $"\n\n\"BURRRIIIIIITOOOO...\" something moans back. Everyone stops and looks around. Suddenly, another fart blasts their nostrils and the culprit reveals themself!\n\nThe Burrito Ghost is real, y'all! Prepare for battle!";

                optionFour = new FightCombatOption(optionTitle, optionResultText, new List<Entity> {new BurritoGhost(), new Ghost(), new Ghost()});
            }
            else
            {
                optionResultText += "\n\nThe mystery remains unsolved.";

                var optionFourPenalty = new Penalty();

                optionFourPenalty.EveryoneLoss(Party, EntityStatTypes.CurrentMorale, 5);

                optionFour = new Option(optionTitle, optionResultText, null, optionFourPenalty, EncounterType.Normal);
            }

            Options.Add(optionTitle, optionFour);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}