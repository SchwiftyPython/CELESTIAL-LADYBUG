using System.Collections.Generic;
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
            //todo this if should probably be a while loop
            while (Self.Stats.CurrentActionPoints < 1)
            {
                if (Target == null || Target.IsDead())
                {
                    Target = FindTarget();
                }

                //todo find distance to target
                var distance = Distance.CHEBYSHEV.Calculate(Self.Position, Target.Position);

                var usableAbilities = new List<Ability>();

                foreach (var ability in Self.Abilities)
                {
                    if (ability.Range >= distance && Self.Stats.CurrentActionPoints >= ability.ApCost)
                    {
                        usableAbilities.Add(ability);
                    }
                }

                //todo if greater than any combat ability range, move
                //todo else attack
                if (usableAbilities.Count < 1)
                {
                    Move();
                }
                else
                {
                    Attack(usableAbilities);
                }
            }

            //todo end turn
            EventMediator.Instance.Broadcast(GlobalHelper.EndTurn, this);
        }

        public void Move()
        {
            //todo if path is not null and target hasn't moved don't calc new path
            //todo get path to target

            _pathToTarget = new List<Tile>();

            var map = CombatManager.Instance.Map;

            var path = map.AStar.ShortestPath(Self.Position, Target.Position);

            //todo get as close as possible
        }

        public void Attack(List<Ability> usableAbilities)
        {
            //todo choose an ability at random and do it

            usableAbilities[Random.Range(0, usableAbilities.Count)].Use(Self, Target);
        }

        private Entity FindTarget()
        {
            var targets = CombatManager.Instance.TurnOrder.ToList();

            foreach (var target in targets)
            {
                if (target.IsPlayer() || target.IsDerpus())
                {
                    continue;
                }

                targets.Remove(target);
            }

            return targets[Random.Range(0, targets.Count)];
        }
    }
}
