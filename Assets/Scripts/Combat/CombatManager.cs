using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Combat
{
    public enum CombatState
    {
        Start,
        PlayerTurn,
        AiTurn,
        EndTurn,
        EndCombat
    }

    public class CombatManager : MonoBehaviour
    {
        private CombatState _currentCombatState;
        private Queue<Entity> _turnOrder;
        private Entity _activeEntity;
        private CombatMap _map;

        private void Awake()
        {
        
        }

        private void Update()
        {
            switch (_currentCombatState)
            {
                case CombatState.Start:
                    var party = TravelManager.Instance.Party.GetCompanions();

                    //todo get enemies

                    //todo combine party and enemies into list

                    //todo determine turn order

                    _activeEntity = _turnOrder.Peek();

                    HighlightActiveEntitySprite();

                    if (ActiveEntityPlayerControlled())
                    {
                        _currentCombatState = CombatState.PlayerTurn;
                    }
                    else
                    {
                        _currentCombatState = CombatState.AiTurn;
                    }
                    break;
                case CombatState.PlayerTurn:
                    UpdateActiveEntityInfoPanel();
                    //todo subscribe to player action event
                    break;
                case CombatState.AiTurn:
                    //todo tell ai to do its thing
                    //todo subscribe to ai action event
                    break;
                case CombatState.EndTurn:
                    RemoveDeadEntities();

                    if (IsCombatFinished())
                    {
                        _currentCombatState = CombatState.EndCombat;
                    }
                    else
                    {
                        _activeEntity = GetNextInTurnOrder();

                        HighlightActiveEntitySprite();

                        if (ActiveEntityPlayerControlled())
                        {
                            _currentCombatState = CombatState.PlayerTurn;
                        }
                        else
                        {
                            _currentCombatState = CombatState.AiTurn;
                        }
                    }
                    break;
                case CombatState.EndCombat:
                    DisplayPostCombatPopup();

                    //todo maybe move this portion to the post combat popup
                    if (PlayerDead())
                    {
                        //todo show popup with button to quit to menu
                    }

                    //todo load travel screen

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private bool PlayerDead()
        {

        }

        private Queue<Entity> DetermineTurnOrder(List<Entity> combatants)
        {
            var initiatives = new SortedDictionary<int, Entity>();

            foreach (var combatant in combatants)
            {
                var roll = Random.Range(1, 21); //todo diceroller

                var initiative = combatant.Stats.Initiative + roll;

                while (initiatives.ContainsKey(initiative))
                {
                    initiative++;
                }

                initiatives.Add(initiative, combatant);
            }

            var turnOrder = new Queue<Entity>();

            foreach (var combatant in initiatives.Values.Reverse())
            {
                turnOrder.Enqueue(combatant);
            }

            return turnOrder;
        }

        private Entity GetNextInTurnOrder()
        {
            var lastEntity = _turnOrder.Dequeue();

            _turnOrder.Enqueue(lastEntity);

            return _turnOrder.Peek();
        }

        private bool ActiveEntityPlayerControlled()
        {
            return _activeEntity.IsPlayer();
        }

        private void HighlightActiveEntitySprite()
        {
            //todo
        }

        private void UpdateActiveEntityInfoPanel()
        {
            //todo
        }

        //todo we can remove the dead entity's portrait from the turn order and skip it when their turn comes around
        private void RemoveDeadEntitiesFromTurnOrderDisplay()
        {
            foreach (var entity in _turnOrder.ToArray())
            {
                if (entity.IsDead())
                {
                    //todo broadcast to remove its portrait and sprite from play.
                    //todo we'll probably do this when they are killed anyways so this is just a cleanup
                }
            }
        }

        private bool IsCombatFinished()
        {
            var playerEntities = false;
            var aiEntities = false;

            foreach (var entity in _turnOrder.ToArray())
            {
                if (entity.IsPlayer())
                {
                    playerEntities = true;
                }
                else
                {
                    aiEntities = true;
                }

                if (playerEntities && aiEntities)
                {
                    return false;
                }
            }

            return true;
        }

        private bool DisplayPostCombatPopup()
        {

        }

        private void GenerateMap()
        {

        }
    }
}
