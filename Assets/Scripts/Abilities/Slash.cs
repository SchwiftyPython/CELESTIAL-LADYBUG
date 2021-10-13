using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Slash : Ability
    {
        public Slash(Entity abilityOwner) : base("Slash", "Slash'm up!", 4, -1, abilityOwner, TargetType.Hostile, false)
        {
        }
    }
}
