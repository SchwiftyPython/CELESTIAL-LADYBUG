using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Necromancer;
using Assets.Scripts.Items;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class CombatTester : MonoBehaviour
    {
        private const int MinBandits = 3;
        private const int MaxBandits = 5;

        public bool testingEnabled;

        private void Start()
        {
            if (!testingEnabled)
            {
                return;
            }

            var spriteStore = FindObjectOfType<SpriteStore>();
            spriteStore.Setup();

            var itemStore = FindObjectOfType<ItemStore>();
            itemStore.Setup();

            var travelManager = FindObjectOfType<TravelManager>();

            travelManager.NewParty();

            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            var bandits = new List<Entity>();

            for (var i = 0; i < 1; i++)
            {
                Entity bandit = new Skeleton();

                bandits.Add(bandit);

                bandit = new Skeleton();

                bandits.Add(bandit);

                bandit = new Skeleton();

                bandits.Add(bandit);

                bandit = new Skeleton();

                bandits.Add(bandit);

                bandit = new Skeleton();

                bandits.Add(bandit);
            }

            var combatManager = FindObjectOfType<CombatManager>();

            combatManager.Enemies = bandits;

            SetAllCompanionsToOneHealth();

            combatManager.Load();
        }

        private void SetAllEnemiesToOneHealth()
        {
            var combatManager = FindObjectOfType<CombatManager>();

            foreach (var enemy in combatManager.Enemies)
            {
                enemy.Stats.CurrentHealth = 1;
            }
        }

        private static void SetAllCompanionsToOneHealth()
        {
            Debug.LogWarning("All companions set to one health for testing.");

            var travelManager = FindObjectOfType<TravelManager>();

            foreach (var companion in travelManager.Party.GetCompanions())
            {
                companion.Stats.CurrentHealth = 1;
            }
        }
    }
}
