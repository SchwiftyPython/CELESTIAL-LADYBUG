using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Necromancer;
using Assets.Scripts.Travel;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Combat
{
    public class TrailOfTheDead : Encounter
    {
        private const int MinZombies = 6;
        private const int MaxZombies = 8;

        public TrailOfTheDead()
        {
            Rarity = Rarity.Uncommon;
            EncounterType = EncounterType.Combat;
            Title = "Trail of the Dead";
            ImageName = "zombie";
            //todo BiomeTypes = new List<BiomeType> { BiomeType.Spooky };
        }

        public override void Run()
        {
            var numBandits = Random.Range(MinZombies, MaxZombies + 1);

            var travelManager = Object.FindObjectOfType<TravelManager>();

            var chosenCompanion = travelManager.Party.GetRandomCompanion();

            Description = $"The party is passing through a particularly dark part of the trail. {chosenCompanion.FirstName()} is nervously snapping their gaze side to side when they notice several figures lumbering towards them! The wagon is surrounded by zombies!";

            var zombies = new List<Entity>();

            for (var i = 0; i < numBandits; i++)
            {
                zombies.Add(new Zombie());
            }

            Options = new Dictionary<string, Option>();

            var optionTitle = "Retreat";

            string optionResultText;

            const int retreatSuccessValue = 47;

            var retreatCheck = Dice.Roll("1d100");

            var retreatSuccess = retreatCheck <= retreatSuccessValue;
            
            if (retreatSuccess)
            {
                optionResultText = "You manage to evade the undead attackers and escape safely.";
            }
            else
            {
                optionResultText = "You try to get away, but the attackers are too fast! Prepare for battle!";
            }

            var retreatOption = new RetreatCombatOption(optionTitle, optionResultText, zombies, retreatSuccess);

            Options.Add(optionTitle, retreatOption);

            optionTitle = "To arms!";

            optionResultText = "Prepare for battle...";

            var fightOption = new FightCombatOption(optionTitle, optionResultText, zombies);

            Options.Add(optionTitle, fightOption);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
