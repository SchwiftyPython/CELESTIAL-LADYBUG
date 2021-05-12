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
        }

        public bool IsLocationDependent()
        {
            return LocationDependent;
        }

        public bool CanStack()
        {
            return Stacks;
        }

        protected override void OnTrigger(EffectArgs e)
        {
        }
    }
}
