using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class Effect : Effect<EffectArgs>
    {
        protected bool LocationDependent;
        protected bool Stacks;
        protected string Description;

        public Sprite Icon { get; protected set; }

        public Effect(string name, string description, int startingDuration, bool locationDependent, bool stacks) : base(name, startingDuration)
        {
            Description = description;
            LocationDependent = locationDependent;
            Stacks = stacks;

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
