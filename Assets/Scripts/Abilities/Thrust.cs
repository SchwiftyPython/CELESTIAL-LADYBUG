using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Thrust : Ability
    {
        public Thrust(Entity abilityOwner) : base("Thrust", "THRUST!", 4, -1, abilityOwner, TargetType.Hostile, false)
        {
        }
    }
}
