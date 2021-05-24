using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class Distracting : Ability, ISubscriber
    {
        private List<Tile> _blindedTiles;
        private readonly Blinded _blindedEffect;

        public Distracting(Entity abilityOwner) : base("Distracting", "Anyone adjacent is Blinded.", -1, 1, abilityOwner, false, true)
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
                tile.RemoveEffect(_blindedEffect);

                _blindedTiles.Remove(tile);
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
            }
        }

        public override void Terminate()
        {
            foreach (var tile in _blindedTiles.ToArray())
            {
                tile.RemoveEffect(_blindedEffect);

                _blindedTiles.Remove(tile);
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
