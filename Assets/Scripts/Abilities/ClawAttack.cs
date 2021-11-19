using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class ClawAttack : Ability
    {
        private const int DamageMin = 2;
        private const int DamageMax = 4;

        public ClawAttack(Entity abilityOwner) : base("Claw Attack", "Slash target with razor sharp talons.", 4, 1, abilityOwner, TargetType.Hostile, false, false)
        {
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }
    }
}
