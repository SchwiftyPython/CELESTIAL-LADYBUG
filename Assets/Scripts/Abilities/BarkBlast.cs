using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class BarkBlast : Ability
    {
        private const int DamageMin = 3;
        private const int DamageMax = 5;

        public BarkBlast(Entity abilityOwner) : base("Bark Blast", "Blasts target with wooden shrapnel at close range.",
            5, 1, abilityOwner, TargetType.Hostile, false, false)
        {
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }
    }
}
