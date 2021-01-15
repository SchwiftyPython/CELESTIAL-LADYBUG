using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Encounters
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

            Description = $"{numBandits} are attacking the wagon! To arms!";

            var bandits = new List<Entity>();

            for (int i = 0; i < numBandits; i++)
            {
                var bandit = new Entity(false);

                bandits.Add(bandit);
            }
        }
    }
}
