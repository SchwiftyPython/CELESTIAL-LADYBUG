using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class Distracting : Ability, ISubscriber
    {
        private List<Tile> _blindedTiles;
        private Blinded _blindedEffect;

        public Distracting(Entity abilityOwner) : base("Distracting", $"Anyone adjacent is Blinded.", -1, 1, abilityOwner, false, true)
        {
            _blindedTiles = new List<Tile>();
            _blindedEffect = new Blinded(true, Blinded.INFINITE);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(GlobalHelper.ActiveEntityMoved, this);
        }

        public override void SetupForCombat()
        {
            UpdateTiles();
        }

        private void UpdateTiles()
        {
            foreach (var tile in _blindedTiles.ToArray())
            {
                //tile.RemoveComponent(_blindedEffect);

                tile.RemoveEffect(_blindedEffect);

                _blindedTiles.Remove(tile);

                var presentEntity = (Entity)tile.CurrentMap.Entities.GetItems(tile.Position).FirstOrDefault();

                if (presentEntity == null)
                {
                    continue;
                }

                foreach (var effect in presentEntity.Effects.ToArray())
                {
                    if (ReferenceEquals(_blindedEffect, effect))
                    {
                        presentEntity.RemoveEffect(effect);
                    }
                }
            }

            var ownerPosition = AbilityOwner.Position;
            var ownerTile = ((CombatMap) AbilityOwner.CurrentMap).GetTileAt(ownerPosition);

            foreach (var tile in ownerTile.GetAdjacentTiles())
            {
                if (!tile.HasEffect(_blindedEffect))
                {
                    tile.AddEffect(_blindedEffect);
                }

                _blindedTiles.Add(tile);

                var presentEntity = (Entity)tile.CurrentMap.Entities.GetItems(tile.Position).FirstOrDefault();

                presentEntity?.ApplyEffect(_blindedEffect);
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
