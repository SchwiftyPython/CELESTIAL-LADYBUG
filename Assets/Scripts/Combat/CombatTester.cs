﻿using System.Collections.Generic;
using Assets.Scripts.Entities;
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

            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            var bandits = new List<Entity>();

            for (var i = 0; i < numBandits; i++)
            {
                var bandit = new Entity(false);

                bandit.GenerateStartingEquipment();

                bandits.Add(bandit);
            }

            CombatManager.Instance.Enemies = bandits;
        }
    }
}
