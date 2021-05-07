using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class CombatTester : MonoBehaviour
    {
        private const int MinBandits = 3;
        private const int MaxBandits = 5;

        public bool TestingEnabled;

        private void Start()
        {
            if (!TestingEnabled)
            {
                return;
            }

            var spriteStore = Object.FindObjectOfType<SpriteStore>();
            spriteStore.Setup();

            var itemStore = Object.FindObjectOfType<ItemStore>();
            itemStore.Setup();

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.NewParty();

            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            var bandits = new List<Entity>();

            for (var i = 0; i < numBandits; i++)
            {
                var bandit = new Entity(false);

                bandits.Add(bandit);
            }

            var combatManager = Object.FindObjectOfType<CombatManager>();

            combatManager.Enemies = bandits;

            combatManager.Load();
        }
    }
}
