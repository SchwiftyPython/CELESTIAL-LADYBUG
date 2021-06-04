using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Bite : Ability
    {
        private const int DamageMin = 2;
        private const int DamageMax = 4;

        public Bite(Entity abilityOwner) : base("Bite", "Teeth meet flesh.", 4, 1, abilityOwner, true, false)
        {
        }

        public Bite(string name, string description, int apCost, Entity abilityOwner) : base(name, description, apCost, 1, abilityOwner, true, false)
        {
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }
    }
}
