using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class DivineIntervention : Ability
    {
        private const int InterventionChance = 2; 

        public DivineIntervention(Entity abilityOwner) : base("Divine Intervention", $"{InterventionChance}% chance an attack that would hit will miss.", -1, -1, abilityOwner, TargetType.Friendly, true)
        {
        }

        public static bool Intervened()
        {
            var roll = Random.Range(1, 101);

            return roll <= InterventionChance;
        }
    }
}
