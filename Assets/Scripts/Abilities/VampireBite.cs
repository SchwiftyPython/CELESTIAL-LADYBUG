using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    //todo would be cool to have a chance to inflict a companion with vampire disease or whatever
    public class VampireBite : Ability, ISubscriber
    {
        private const int DamageMin = 3;
        private const int DamageMax = 6;

        public VampireBite(Entity abilityOwner) : base("Vampire Bite", "Vampire gains life equal to damage dealt to target.", 6, 1, abilityOwner, true, false, false)
        {
        }

        public override void Use(Entity target)
        {
            var message = $"{AbilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            eventMediator.SubscribeToEvent(GlobalHelper.DamageDealt, this);

            AbilityOwner.AttackWithAbility(target, this);

            AbilityOwner.SubtractActionPoints(ApCost);

            eventMediator.UnsubscribeFromEvent(GlobalHelper.DamageDealt, this);
        }

        public override (int, int) GetAbilityDamageRange()
        {
            return (DamageMin, DamageMax);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.DamageDealt))
            {
                if (parameter == null)
                {
                    return;
                }

                var damage = Mathf.Abs((int) parameter);

                AbilityOwner.AddHealth(damage);

                var message = $"{AbilityOwner.Name} gains {damage} health!";

                var eventMediator = Object.FindObjectOfType<EventMediator>();

                eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
            }
        }
    }
}
