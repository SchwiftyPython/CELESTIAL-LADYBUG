using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Necromancer;
using Assets.Scripts.Entities.Rampart;
using Assets.Scripts.Entities.Stronghold;
using Assets.Scripts.Entities.Tower;
using Assets.Scripts.Items;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class CombatTester : MonoBehaviour
    {
        private const int MinBandits = 3;
        private const int MaxBandits = 5;

        public BiomeType testingBiome;

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

            travelManager.CurrentBiome = testingBiome;

            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            var bandits = new List<Entity>();

            for (var i = 0; i < 1; i++)
            {
                Entity bandit = new Ghost();

                bandits.Add(bandit);

                bandit = new Golem();

                bandits.Add(bandit);

                bandit = new Lion();

                bandits.Add(bandit);

                bandit = new Mage();

                bandits.Add(bandit);

                bandit = new Minotaur();

                bandits.Add(bandit);
            }

            var combatManager = FindObjectOfType<CombatManager>();

            combatManager.Enemies = bandits;

            combatManager.LoadCombatScene();
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
