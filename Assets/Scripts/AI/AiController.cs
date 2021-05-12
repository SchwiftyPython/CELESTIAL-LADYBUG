﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.AI
{
    //todo this is just prototype dumb dumb ai - but if it works then don't fix it :)
    public class AiController : MonoBehaviour
    {
        private List<Tile> _pathToTarget;
        private bool _actionAvailable;

        public Entity Self { get; private set; }
        public Entity Target { get; private set; }

        public void SetSelf(Entity self)
        {
            Self = self;
        }

        public void SetTarget(Entity target)
        {
            Target = target;
        }

        public void TakeTurn()
        {
            _actionAvailable = true;

            while (Self.Stats.CurrentActionPoints > 0 && _actionAvailable)
            {
                if (Target == null || Target.IsDead())
                {
                    Target = FindTarget();
                }

                if (Target == null)
                {
                    _actionAvailable = false;
                    continue;
                }

                var distance = Distance.CHEBYSHEV.Calculate(Self.Position, Target.Position);

                var usableAbilities = new List<Ability>();

                foreach (var ability in Self.Abilities.Values)
                {
                    if (ability.IsPassive || ability.Range < distance)
                    {
                        continue;
                    }

                    if ((ability.Range == 1 || ability.Range > 1 && Self.HasMissileWeaponEquipped()) && Self.Stats.CurrentActionPoints >= ability.ApCost)
                    {
                        usableAbilities.Add(ability);
                    }
                }

                if (usableAbilities.Count < 1)
                {
                    Move();
                }
                else
                {
                    Attack(usableAbilities);
                }
            }

            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EndTurn, this);
        }

        private void Move()
        {
            //todo if path is not null and target hasn't moved don't calc new path
            //todo get path to target

            _pathToTarget = new List<Tile>();

            var combatManager = FindObjectOfType<CombatManager>();
            var map = combatManager.Map;

            //todo either we have to get a tile adjacent to the target or stop from moving onto the target's tile
            //todo not sure how that is happening - thought GoRogue would prevent that -- The sprite is moving but not the entity?
            var path = map.AStar.ShortestPath(Self.Position, Target.Position);

            //todo take next step

            var tileStep = map.GetTerrain<Floor>(path.GetStep(0));

            if (Self.Stats.CurrentActionPoints < tileStep.ApCost)
            {
                _actionAvailable = false;
                return;
            }

            Self.MoveTo(tileStep, tileStep.ApCost);
        }

        private void Attack(List<Ability> usableAbilities)
        {
            //todo choose a hostile ability at random and do it

            usableAbilities[Random.Range(0, usableAbilities.Count)].Use(Target);
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
    }
}
