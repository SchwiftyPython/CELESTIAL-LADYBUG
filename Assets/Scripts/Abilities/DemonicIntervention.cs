using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class DemonicIntervention : Ability
    {
        private const int InterventionChance = 25;

        public DemonicIntervention(Entity abilityOwner) : base("Demonic Intervention", $"{InterventionChance}% chance that damage dealt to you is also dealt to the attacker", -1, -1, abilityOwner, true, true)
        {
        }

        public static bool Intervened()
        {
            var roll = Random.Range(1, 101);

            return roll <= InterventionChance;
        }
    }
}
