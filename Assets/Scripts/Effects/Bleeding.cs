using Assets.Scripts.Effects.Args;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class Bleeding : Effect
    {
        private const int BleedDuration = 4;
        private const int Damage = 5;

        public Bleeding()
        {
        }

        public Bleeding(Entity owner, int duration = BleedDuration) : base("Bleeding", $"Deals {Damage} damage each turn.", duration,
            false, false, TargetType.All, owner)
        {
        }

        public override void Trigger(EffectArgs args)
        {
            if (Duration != INFINITE)
            {
                Duration--;
            }

            if (!(args is BasicEffectArgs basicEffectArgs))
            {
                return;
            }

            var message = $"{basicEffectArgs.Target.Name} takes {Damage} damage from Bleeding!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            basicEffectArgs.Target.SubtractHealth(Damage);
        }
    }
}
