using Assets.Scripts.Entities;
using GoRogue;

namespace Assets.Scripts.Effects.Args
{
    public class BasicEffectArgs : EffectArgs
    {
        public Entity Target;

        public BasicEffectArgs(Entity target)
        {
            Target = target;
        }
    }
}
