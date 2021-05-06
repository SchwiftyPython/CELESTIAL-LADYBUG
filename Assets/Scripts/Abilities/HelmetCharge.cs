using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class HelmetCharge : Ability
    {
        public HelmetCharge(Entity abilityOwner) : base("Helmet Charge", "Charge headfirst into an enemy.", 3, 3, abilityOwner, true, false)
        {
        }

        public override void Use(Entity target)
        {
            var message = $"{AbilityOwner.Name} attacks {target.Name} with {GlobalHelper.CapitalizeAllWords(Name)}!";

            EventMediator.Instance.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            var chargeDirection = Direction.GetDirection(AbilityOwner.Position, target.Position);

            var map = CombatManager.Instance.Map;

            var targetTile = map.GetTileAt(target.Position);

            Tile destination = null;

            switch (chargeDirection.Type)
            {
                case Direction.Types.UP:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.DOWN);
                    break;
                case Direction.Types.UP_RIGHT:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.DOWN_LEFT);
                    break;
                case Direction.Types.RIGHT:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.LEFT);
                    break;
                case Direction.Types.DOWN_RIGHT:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.UP_LEFT);
                    break;
                case Direction.Types.DOWN:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.UP);
                    break;
                case Direction.Types.DOWN_LEFT:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.UP_RIGHT);
                    break;
                case Direction.Types.LEFT:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.RIGHT);
                    break;
                case Direction.Types.UP_LEFT:
                    destination = targetTile.GetAdjacentTileByDirection(Direction.DOWN_RIGHT);
                    break;
                case Direction.Types.NONE:
                    Debug.LogError("No direction for Helmet Charge!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            AbilityOwner.MoveTo(destination, 0);

            AbilityOwner.MeleeAttackWithSlot(target, EquipLocation.Helmet);
        }
    }
}
