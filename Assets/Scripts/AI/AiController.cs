using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
using GoRogue.Pathing;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.AI
{
    //todo this is just prototype dumb dumb ai - but if it works then don't fix it :)
    public class AiController : MonoBehaviour
    {
        private const int MaxTries = 2;

        private List<Action> _retreatMethods;
        private Action _retreatMethod;

        private Queue<Coord> _pathToTarget;
        private bool _actionAvailable;
        private bool _fleeing;

        public bool animating;

        public Entity Self { get; private set; }
        public Entity TargetEntity { get; private set; }

        private void Awake()
        {
            _retreatMethods = new List<Action> {Retreat, EnemyDistanceRetreat, RandomRetreat};
        }

        public void SetSelf(Entity self)
        {
            Self = self;
        }

        public void SetTarget(Entity target)
        {
            TargetEntity = target;
        }

        public void Flee()
        {
            _fleeing = true;
        }

        public void StopFleeing()
        {
            _fleeing = false;
        }
        
        public IEnumerator TakeTurn() //todo might have to re-structure this so it's not working like a loop
        {
            _actionAvailable = true;

            var currentTry = 0;

            while (Self.Stats.CurrentActionPoints > 0 && _actionAvailable && currentTry < MaxTries)
            {
                if (_fleeing) 
                {
                    if (_retreatMethod == null)
                    {
                        _retreatMethod = _retreatMethods[Random.Range(0, _retreatMethods.Count)];
                    }

                    _retreatMethod.Invoke();
                }

                var combatManager = FindObjectOfType<CombatManager>();
                var map = combatManager.Map;

                if (TargetEntity == null || TargetEntity.IsDead() || map.OutOfBounds(TargetEntity.Position))
                {
                    TargetEntity = FindTarget();
                }

                if (TargetEntity == null)
                {
                    _actionAvailable = false;
                    continue;
                }

                var distance = Distance.CHEBYSHEV.Calculate(Self.Position, TargetEntity.Position);

                var usableAbilities = new List<Ability>();

                foreach (var ability in Self.Abilities.Values)
                {
                    if (ability.IsPassive || ability.Range < distance)
                    {
                        continue;
                    }

                    if ((ability.Range == 1 || Self.HasMissileWeaponEquipped() || !ability.UsesEquipment) && Self.Stats.CurrentActionPoints >= ability.ApCost)
                    {
                        usableAbilities.Add(ability);
                    }
                }

                if (usableAbilities.Count < 1)
                {
                    MoveToTargetEntity();
                }
                else
                {
                    Attack(usableAbilities);
                }

                currentTry++;

                while (animating)
                {
                    yield return null;
                }
            }

            while (animating)
            {
                yield return null;
            }

            EndTurn();
        }

        private void EndTurn()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EndTurn, this);
        }

        private void MoveToTargetEntity()
        {
            //todo if path is not null and target hasn't moved don't calc new path

            var combatManager = FindObjectOfType<CombatManager>();
            var map = combatManager.Map;

            var path = map.AStar.ShortestPath(Self.Position, TargetEntity.Position);

            if (path == null || path.Length < 1)
            {
                _actionAvailable = false;
                return;
            }

            var tileStep = map.GetTerrain<Floor>(path.GetStep(0));

            if (Self.Stats.CurrentActionPoints < tileStep.ApCost || !tileStep.IsWalkable || !map.WalkabilityView[tileStep.Position])
            {
                _actionAvailable = false;
                return;
            }

            Self.MoveTo(tileStep, tileStep.ApCost);
        }

        private void Retreat()
        {
            var closest = new Tuple<Tile, int, int>(null, int.MaxValue, int.MinValue);

            var combatManager = FindObjectOfType<CombatManager>();
            var map = combatManager.Map;

            var retreatTiles = map.GetRetreatTiles();
            var enemies = combatManager.TurnOrder.Where(e => e.IsPlayer() != Self.IsPlayer()).ToArray();

            Path path = null;
            foreach (var tile in retreatTiles)
            {
                path = map.AStar.ShortestPath(Self.Position, tile.Position);

                if (path == null || path.Length < 1)
                {
                    continue;
                }

                if (path.Length < closest.Item2)
                {
                    var closestEnemyToTile = new Tuple<Entity, int>(null, int.MaxValue);

                    foreach (var enemy in enemies)
                    {
                        var ePath = map.AStar.ShortestPath(tile.Position, enemy.Position);

                        if (ePath == null || ePath.Length < 1)
                        {
                            continue;
                        }

                        if (ePath.Length < closestEnemyToTile.Item2)
                        {
                            closestEnemyToTile = new Tuple<Entity, int>(enemy, ePath.Length);
                        }
                    }

                    if (closestEnemyToTile.Item2 > closest.Item3)
                    {
                        closest = new Tuple<Tile, int, int>(tile, path.Length, closestEnemyToTile.Item2);
                    }
                }
            }

            if (path == null)
            {
                _retreatMethod = RandomRetreat;
                _actionAvailable = false;
                return;
            }

            var tileStep = map.GetTerrain<Floor>(path.GetStep(0));

            if (Self.Stats.CurrentActionPoints < tileStep.ApCost || !tileStep.IsWalkable || !map.WalkabilityView[tileStep.Position])
            {
                _actionAvailable = false;
                return;
            }

            Self.MoveTo(tileStep, tileStep.ApCost);
        }

        //testing algorithm where path length to tile isn't considered. Only distance from enemy
        private void EnemyDistanceRetreat()
        {
            var farthestFromEnemy = new Tuple<Tile, int>(null, int.MinValue);

            var combatManager = FindObjectOfType<CombatManager>();
            var map = combatManager.Map;

            var retreatTiles = map.GetRetreatTiles();
            var enemies = combatManager.TurnOrder.Where(e => e.IsPlayer() != Self.IsPlayer()).ToArray();

            Path path = null;
            foreach (var tile in retreatTiles)
            {
                path = map.AStar.ShortestPath(Self.Position, tile.Position);

                if (path == null || path.Length < 1)
                {
                    continue;
                }

                var farthestEnemyToTile = int.MinValue;

                foreach (var enemy in enemies)
                {
                    var ePath = map.AStar.ShortestPath(tile.Position, enemy.Position);

                    if (ePath == null || ePath.Length < 1)
                    {
                        continue;
                    }

                    if (ePath.Length > farthestEnemyToTile)
                    {
                        farthestEnemyToTile = ePath.Length;
                    }
                }

                if (farthestEnemyToTile > farthestFromEnemy.Item2)
                {
                    farthestFromEnemy = new Tuple<Tile, int>(tile, farthestEnemyToTile);
                }
            }

            if (path == null)
            {
                _retreatMethod = RandomRetreat;
                _actionAvailable = false;
                return;
            }

            var tileStep = map.GetTerrain<Floor>(path.GetStep(0));

            if (Self.Stats.CurrentActionPoints < tileStep.ApCost || !tileStep.IsWalkable || !map.WalkabilityView[tileStep.Position])
            {
                _actionAvailable = false;
                return;
            }

            Self.MoveTo(tileStep, tileStep.ApCost);
        }

        private void RandomRetreat()
        {
            var combatManager = FindObjectOfType<CombatManager>();
            var map = combatManager.Map;

            if (_pathToTarget == null || _pathToTarget.Count < 1)
            {
                _pathToTarget = new Queue<Coord>();

                var retreatTiles = map.GetRetreatTiles();

                var tile = retreatTiles[Random.Range(0, retreatTiles.Count)];

                var path = map.AStar.ShortestPath(Self.Position, tile.Position);

                while (path == null || path.Length < 1)
                {
                    retreatTiles.Remove(tile);

                    tile = retreatTiles[Random.Range(0, retreatTiles.Count)];

                    path = map.AStar.ShortestPath(Self.Position, tile.Position);
                }

                _pathToTarget = new Queue<Coord>(path.Steps);
            }

            var tileStep = map.GetTerrain<Floor>(_pathToTarget.Dequeue());

            if (Self.Stats.CurrentActionPoints < tileStep.ApCost || !tileStep.IsWalkable || !map.WalkabilityView[tileStep.Position])
            {
                _actionAvailable = false;
                return;
            }

            Self.MoveTo(tileStep, tileStep.ApCost);
        }

        private void Attack(List<Ability> usableAbilities)
        {
            //todo choose a hostile ability at random and do it

            usableAbilities[Random.Range(0, usableAbilities.Count)].Use(TargetEntity);
        }

        private static Entity FindTarget()
        {
            var combatManager = FindObjectOfType<CombatManager>();
            var targets = combatManager.TurnOrder.ToList();

            foreach (var target in targets.ToArray())
            {
                if (target.IsPlayer() || target.IsDerpus())
                {
                    continue;
                }

                targets.Remove(target);
            }

            if (targets.Count < 1)
            {
                return null;
            }

            return targets[Random.Range(0, targets.Count)];
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.00f);
        }
    }
}
