using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Necromancer;
using UnityEngine;

namespace Assets.Scripts.Encounters.Combat
{
    public class BanditAttack : Encounter
    {
        private const int MinBandits = 3;
        private const int MaxBandits = 5;

        public BanditAttack()
        {
            Rarity = Rarity.Common;
            EncounterType = EncounterType.Combat;
            Title = "Bandit Attack";
        }

        public override void Run()
        {
            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            Description = $"{numBandits} bandits are attacking the wagon!";

            var bandits = new List<Entity>();

            for (var i = 0; i < numBandits; i++)
            {
                var bandit = new Skeleton(); //todo change

                bandits.Add(bandit);
            }

            Options = new Dictionary<string, Option>();

            var optionTitle = "Retreat";

            string optionResultText;

            const int retreatSuccessValue = 47;

            //todo diceroller
            var retreatCheck = Random.Range(1, 101);

            var retreatSuccess = retreatCheck <= retreatSuccessValue;

            Debug.Log($"Value Needed: {retreatSuccessValue}");
            Debug.Log($"Rolled: {retreatCheck}");

            if (retreatSuccess)
            {
                optionResultText = "You manage to evade the attackers and escape safely.";
            }
            else
            {
                optionResultText = "You try to get away, but the attackers are too fast! Prepare for battle!";
            }

            var retreatOption = new RetreatCombatOption(optionTitle, optionResultText, bandits, retreatSuccess);

            Options.Add(optionTitle, retreatOption);

            optionTitle = "To arms!";

            optionResultText = "You prepare for battle...";

            var fightOption = new FightCombatOption(optionTitle, optionResultText, bandits);

            Options.Add(optionTitle, fightOption);

            SubscribeToOptionSelectedEvent();

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.FourOptionEncounter, this);
        }
    }
}
