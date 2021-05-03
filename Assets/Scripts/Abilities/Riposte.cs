using Assets.Scripts.Entities;

namespace Assets.Scripts.Abilities
{
    public class Riposte : Ability
    {
        //todo no idea if this works yet -- UNTESTED

        public Riposte(Entity abilityOwner) : base("Riposte", "Automatically counter-attack with equipped weapon.", -1, 1, abilityOwner, true, true)
        {
            Icon = SpriteStore.Instance.GetAbilitySprite(this);
        }

        public override void Use(Entity target)
        {
            AbilityOwner.ApplyDamage(target);
        }
    }
}
