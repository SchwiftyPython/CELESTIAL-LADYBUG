﻿using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class Intimidate : Ability, ISubscriber
    {
        private List<Tile> _fearTiles;
        private readonly Fear _fearEffect;

        public Intimidate(Entity abilityOwner) : base("Intimidate", "Anyone adjacent gains Fear effect.", -1, 1, abilityOwner, true, true)
        {
            _fearTiles = new List<Tile>();
            _fearEffect = new Fear(true, Fear.INFINITE);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(GlobalHelper.ActiveEntityMoved, this);
        }

        public override void SetupForCombat()
        {
            UpdateTiles();
        }

        private void UpdateTiles()
        {
            foreach (var tile in _fearTiles.ToArray())
            {
                tile.RemoveEffect(_fearEffect);

                _fearTiles.Remove(tile);
            }

            var ownerPosition = AbilityOwner.Position;
            var ownerTile = ((CombatMap)AbilityOwner.CurrentMap).GetTileAt(ownerPosition);

            foreach (var tile in ownerTile.GetAdjacentTiles())
            {
                if (!tile.HasEffect(_fearEffect))
                {
                    tile.AddEffect(_fearEffect);
                }

                _fearTiles.Add(tile);
            }
        }

        public override void Terminate()
        {
            foreach (var tile in _fearTiles.ToArray())
            {
                tile.RemoveEffect(_fearEffect);

                _fearTiles.Remove(tile);
            }
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.ActiveEntityMoved))
            {
                if (!(broadcaster is Entity abilityOwner) || !ReferenceEquals(AbilityOwner, abilityOwner))
                {
                    return;
                }

                UpdateTiles();
            }
        }
    }
}