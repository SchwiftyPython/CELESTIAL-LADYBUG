using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Abilities
{
    public class Cleave : Ability, IModifierProvider, ISubscriber
    {
        private const int DamageMod = 5;

        private readonly Bleeding _bleedingEffect;

        public Cleave(Entity abilityOwner) : base("Cleave", $"Deals {DamageMod}% extra damage and bleeding effect.", 6, 1, abilityOwner, true, false)
        {
            _bleedingEffect = new Bleeding();
        }

        public override void Use(Entity target)
        {
            var message = $"{AbilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            eventMediator.SubscribeToEvent(GlobalHelper.TargetHit, this);

            AbilityOwner.MeleeAttack(target, this);

            AbilityOwner.SubtractActionPoints(ApCost);

            eventMediator.UnsubscribeFromEvent(GlobalHelper.TargetHit, this);
        }

        public float GetAdditiveModifiers(Enum stat)
        {
            return 0f;
        }

        public float GetPercentageModifiers(Enum stat)
        {
            if (!stat.GetType().Name.Equals(nameof(CombatModifierTypes)))
            {
                return 0f;
            }

            if (!Enum.TryParse(stat.ToString(), out CombatModifierTypes statType))
            {
                return 0f;
            }

            if (statType == CombatModifierTypes.Damage)
            {
                return DamageMod;
            }

            return 0f;
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
