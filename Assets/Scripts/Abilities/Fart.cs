using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using GoRogue.DiceNotation;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class Fart : Ability, ISubscriber
    {
        private const int DamageMin = 1;
        private const int DamageMax = 5;

        private const int StunChance = 75;

        private readonly Effect _stunEffect;

        public Fart(Entity abilityOwner) : base("Fart", $"Blast lethal methane at an unlucky foe. {StunChance}% chance to Stun.", 4, 2, abilityOwner, TargetType.Hostile, false, false)
        {
            _stunEffect = new Stun(abilityOwner);

            //todo fart sound overrides ghost attack sound
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }

        public override void Use(Entity target)
        {
            var message = $"{AbilityOwner.Name} farts on {target.Name}!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            eventMediator.SubscribeToEvent(GlobalHelper.TargetHit, this);

            AbilityOwner.AttackWithAbility(target, this);

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

                var roll = Dice.Roll("1d100");

                if (roll > StunChance)
                {
                    return;
                }

                target.ApplyEffect(_stunEffect);

                var message = $"{target.Name} is stunned!";

                var eventMediator = Object.FindObjectOfType<EventMediator>();
                eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
            }
        }
    }
}
