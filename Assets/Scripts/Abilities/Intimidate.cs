using System.Collections.Generic;
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

        //todo maybe list of effect exemptions in ability class?

        public Intimidate(Entity abilityOwner) : base("Intimidate", "Anyone adjacent gains Fear effect.", -1, 1, abilityOwner, TargetType.Hostile, false, true)
        {
            _fearTiles = new List<Tile>();
            _fearEffect = new Fear(abilityOwner, true, Fear.INFINITE);

            EffectExemptions = new List<Effect>{_fearEffect};

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

            var map = (CombatMap) AbilityOwner.CurrentMap;

            if(map == null)
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
