using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class SpookyTouch : Ability
    {
        private const int DamageMin = 3;
        private const int DamageMax = 7;

        public SpookyTouch(Entity abilityOwner) : base("Spooky Touch", "Target experiences an intense jolt of pain throughout their body!", 4, 1, abilityOwner, true, false)
        {
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }
    }
}
