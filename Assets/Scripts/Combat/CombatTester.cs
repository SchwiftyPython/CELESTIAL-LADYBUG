using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Names;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class CombatTester : MonoBehaviour
    {
        private const int MinBandits = 3;
        private const int MaxBandits = 5;

        private void Start()
        {
            var numBandits = Random.Range(MinBandits, MaxBandits + 1);

            var bandits = new List<Entity>();

            for (var i = 0; i < numBandits; i++)
            {
                var bandit = new Entity(false);

                bandits.Add(bandit);
            }

            CombatManager.Instance.Enemies = bandits;
        }
    }
}
