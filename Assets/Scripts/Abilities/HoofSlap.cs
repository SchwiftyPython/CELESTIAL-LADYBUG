using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class HoofSlap : Ability
    {
        private const int DamageMin = 2;
        private const int DamageMax = 4;

        public HoofSlap(Entity abilityOwner) : base("Hoof Slap", "Catch these hooves!", 4, 1, abilityOwner, TargetType.Hostile, false, false)
        {
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }
    }
}
