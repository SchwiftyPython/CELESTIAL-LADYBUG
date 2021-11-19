using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Riposte : Ability
    {
        //todo no idea if this works yet -- UNTESTED

        public Riposte(Entity abilityOwner) : base("Riposte", "Automatically counter-attack with equipped weapon.", -1, 1, abilityOwner, TargetType.Hostile, true)
        {
        }

        public override void Use(Entity target)
        {
            AbilityOwner.ApplyDamageWithEquipment(target, false, 0, false);
        }
    }
}
