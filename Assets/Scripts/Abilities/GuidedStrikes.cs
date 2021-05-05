using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class GuidedStrikes : Ability
    {
        private const int ToHitBonus = 3;
    
        public GuidedStrikes(Entity abilityOwner) : base("Guided Strikes", $"+{ToHitBonus} to hit on melee attacks.", -1, -1, abilityOwner, false, true)
        {
        }

        public static int GetToHitBonus()
        {
            return ToHitBonus;
        }
    }
}
