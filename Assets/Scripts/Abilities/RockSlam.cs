using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class RockSlam : Ability
    {
        private const int DamageMin = 3;
        private const int DamageMax = 5;

        public RockSlam(Entity abilityOwner) : base("Rock Slam", "Rock the opposition with stone fists.", 5, 1, abilityOwner, TargetType.Hostile, false, false)
        {
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }
    }
}
