using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Effects
{
    public class Effect : Effect<EffectArgs>
    {
        protected bool LocationDependent;
        protected bool Stacks;
        protected string Description;
        protected TargetType TargetType;
        [ES3NonSerializable] protected Entity Owner;

        public Sprite Icon { get; protected set; }

        public Effect() : base(string.Empty, -1)
        {
        }

        public Effect(string name, string description, int startingDuration, bool locationDependent, bool stacks, TargetType targetType, Entity owner) : base(name, startingDuration)
        {
            Description = description;
            LocationDependent = locationDependent;
            Stacks = stacks;
            TargetType = targetType;
            SetOwner(owner);

            GetIconForEffect(this);
        }

        public bool IsLocationDependent()
        {
            return LocationDependent;
        }

        public bool CanStack()
        {
            return Stacks;
        }

        public string GetDescription()
        {
            return Description;
        }

        public Entity GetOwner()
        {
            return Owner;
        }

        public void SetOwner(Entity owner)
        {
            Owner = owner;
        }

        public TargetType GetTargetType()
        {
            return TargetType;
        }

        public new virtual void Trigger(EffectArgs args)
        {
        }

        protected override void OnTrigger(EffectArgs e)
        {
        }

        private void GetIconForEffect(Effect effect)
        {
            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            Icon = spriteStore.GetEffectSprite(effect);
        }
    }
}
