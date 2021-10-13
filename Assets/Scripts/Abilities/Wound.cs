using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class Wound : Ability, ISubscriber
    {
        private Effect _bleedingEffect;

        public Wound(Entity abilityOwner) : base("Wound", $"Causes target to bleed.", 6, -1, abilityOwner, TargetType.Hostile, false)
        {
            _bleedingEffect = new Bleeding(abilityOwner);
        }

        public override void Use(Entity target)
        {
            var message = $"{AbilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            eventMediator.SubscribeToEvent(GlobalHelper.TargetHit, this);

            AbilityOwner.MeleeAttack(target);

            AbilityOwner.SubtractActionPoints(ApCost);

            eventMediator.UnsubscribeFromEvent(GlobalHelper.TargetHit, this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.TargetHit))
            {
                var target = parameter as Entity;

                if (target == null)
                {
                    return;
                }

                target.ApplyEffect(_bleedingEffect);

                var message = $"{target.Name} is bleeding!";

                var eventMediator = Object.FindObjectOfType<EventMediator>();
                eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
            }
        }
    }
}
