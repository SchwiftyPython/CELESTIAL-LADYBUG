using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Necromancer;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Encounters.Combat
{
    public class SpookyRandomEncounter : Encounter
    {
        private readonly Dictionary<string, (int, int)> _enemyTypes = new Dictionary<string, (int, int)>
        {
            { "zombie", (2, 6) },
            { "skeleton", (2, 6) },
            { "ghost", (2, 4) },
            { "spider", (2, 6) }
        };

        public SpookyRandomEncounter()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Combat;
            //todo BiomeTypes = new List<BiomeType> { BiomeType.Spooky };
        }

        public override void Run()
        {
            var eTypeIndex = Random.Range(0, _enemyTypes.Count);

            var eType = _enemyTypes.ElementAt(eTypeIndex);

            var enemies = new List<Entity>();

            var numEnemies = Random.Range(eType.Value.Item1, eType.Value.Item2);

            for (var i = 0; i < numEnemies; i++)
            {
                Entity enemy = null;
                switch (eType.Key)
                {
                    case "zombie":
                        enemy = new Zombie();
                        break;
                    case "skeleton":
                        enemy = new Skeleton();
                        break;
                    case "ghost":
                        enemy = new Ghost();
                        break;
                    case "spider":
                        enemy = new Spider();
                        break;
                }

                enemies.Add(enemy);
            }

            Title = $"{GlobalHelper.Capitalize(eType.Key)}s!";

            Description = $"You've run into a wandering group of {eType.Key}s! There looks to be {numEnemies} of them.";

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

            var retreatOption = new RetreatCombatOption(optionTitle, optionResultText, enemies, retreatSuccess);

            Options.Add(optionTitle, retreatOption);

            optionTitle = "To arms!";

            optionResultText = "Prepare for battle...";

            var fightOption = new FightCombatOption(optionTitle, optionResultText, enemies);

            Options.Add(optionTitle, fightOption);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
