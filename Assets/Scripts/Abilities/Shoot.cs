using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Shoot : Ability
    {
        public Shoot(Entity abilityOwner) : base("Shoot", "Shoot with no penalty or benefit.", 5, 7, abilityOwner, TargetType.Hostile,
            false)
        {
        }
    }
}
