using System;
using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Bite : Ability
    {
        private const int DamageMin = 2;
        private const int DamageMax = 4;

        public Bite(Entity abilityOwner) : base("Bite", string.Empty, 4, 1, abilityOwner, true, false)
        {
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }
    }
}
