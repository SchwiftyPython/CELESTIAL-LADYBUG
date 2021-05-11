using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Abilities
{
    public class Knockback : Ability, ISubscriber
    {
        private const int KnockbackDistance = 1;

        public Knockback(Entity abilityOwner) : base("Knockback", $"Knocks target back {KnockbackDistance} tile.", 7,
            7, abilityOwner, true, false)
        {
        }

        public override void Use(Entity target)
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(GlobalHelper.TargetHit, this);

            base.Use(target);

            eventMediator.UnsubscribeFromEvent(GlobalHelper.TargetHit, this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.TargetHit))
            {
                if (!(parameter is Entity target))
                {
                    return;
                }

                var direction = Direction.GetDirection(AbilityOwner.Position, target.Position);

                var combatManager = Object.FindObjectOfType<CombatManager>();

                var map = combatManager.Map;

                var targetTile = map.GetTileAt(target.Position);

                var destination = targetTile.GetAdjacentTileByDirection(direction);

                target.MoveTo(destination, 0); //todo some kind of woosh effect would be cool here
            }
        }
    }
}
