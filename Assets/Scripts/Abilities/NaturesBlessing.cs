using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class NaturesBlessing : Ability, ISubscriber
    {
        private List<Tile> _blessedTiles;
        private readonly Nimble _nimbleEffect;

        public NaturesBlessing(Entity abilityOwner) : base("Nature's Blessing", "All adjacent allies gain Nimble effect.", -1, 1, abilityOwner, TargetType.Friendly, true, false)
        {
            _blessedTiles = new List<Tile>();
            _nimbleEffect = new Nimble(abilityOwner, true, Fear.INFINITE);

            EffectExemptions = new List<Effect> { _nimbleEffect };

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(GlobalHelper.ActiveEntityMoved, this);
        }

        public override void SetupForCombat()
        {
            UpdateTiles();
        }

        private void UpdateTiles()
        {
            foreach (var tile in _blessedTiles.ToArray())
            {
                tile.RemoveEffect(_nimbleEffect);

                _blessedTiles.Remove(tile);
            }

            var ownerPosition = AbilityOwner.Position;

            var map = (CombatMap)AbilityOwner.CurrentMap;

            if (map == null)
            {
                var combatManager = Object.FindObjectOfType<CombatManager>();

                map = combatManager.Map;
            }

            var ownerTile = map.GetTileAt(ownerPosition);

            if (ownerTile == null)
            {
                return;
            }

            foreach (var tile in ownerTile.GetAdjacentTiles())
            {
                if (!tile.HasEffect(_nimbleEffect))
                {
                    tile.AddEffect(_nimbleEffect);
                }

                _blessedTiles.Add(tile);
            }
        }

        public override void Terminate()
        {
            foreach (var tile in _blessedTiles.ToArray())
            {
                tile.RemoveEffect(_nimbleEffect);

                _blessedTiles.Remove(tile);
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
