using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class ShieldBash : Ability, ISubscriber
    {
        public ShieldBash(Entity abilityOwner) : base("Shield Bash", "Knocks target back 1 tile.", 4, 1, abilityOwner,
            TargetType.Hostile, false)
        {
        }

        public override void Use(Entity target)
        {
            var message = $"{AbilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            eventMediator.SubscribeToEvent(GlobalHelper.TargetHit, this);

            AbilityOwner.MeleeAttackWithSlot(target, EquipLocation.Shield);

            AbilityOwner.SubtractActionPoints(ApCost);

            eventMediator.UnsubscribeFromEvent(GlobalHelper.TargetHit, this);
        }

        public override (int, int) GetAbilityDamageRange()
        {
            var shield = AbilityOwner.GetEquippedItemInSlot(EquipLocation.Shield);

            int damageMin;
            int damageMax;

            (damageMin, damageMax) = shield.GetMeleeDamageRange();

            return (damageMin, damageMax);
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

                GlobalHelper.InvokeAfterDelay(() => target.MoveTo(destination, 0, false), 1f);
            }
        }
    }
}
