using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Calculated : Ability
    {
        private const int ToHitBonus = 3;

        public Calculated(Entity abilityOwner) : base("Calculated", $"+{ToHitBonus} to hit on ranged attacks.", -1, -1, abilityOwner, false, true)
        {
        }

        public static int GetToHitBonus()
        {
            return ToHitBonus;
        }
    }
}
