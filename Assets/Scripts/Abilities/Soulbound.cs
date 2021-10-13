using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Soulbound : Ability
    {
        //todo a tiny badge for this on the item rather than in the ability bar

        public Soulbound(Entity abilityOwner) : base("Soulbound", "Item cannot be unequipped.", -1, -1, abilityOwner, TargetType.Friendly, true)
        {
            
        }
    }
}
