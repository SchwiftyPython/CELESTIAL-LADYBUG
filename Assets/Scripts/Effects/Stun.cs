using Assets.Scripts.Effects.Args;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class Stun : Effect
    {
        private const int StunDuration = 1;

        public Stun()
        {
        }

        public Stun(Entity owner, int duration = StunDuration) : base("Stun", "Target cannot move or attack.", duration, false, false, TargetType.Hostile, owner)
        {
        }

        public override void Trigger(EffectArgs args)
        {
            if (Duration != INFINITE)
            {
                Duration--;
            }

            var basicEffectArgs = args as BasicEffectArgs;

            if (basicEffectArgs == null)
            {
                return;
            }

            var message = $"{basicEffectArgs.Target.Name} is stunned!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            //todo might need delay
            //todo need to override any effect that would cause someone to run or attack like Panic

            eventMediator.Broadcast(GlobalHelper.EndTurn, this);
        }
    }
}
