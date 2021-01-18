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
        Loading,
        Start,
        PlayerTurn,
        AiTurn,
        EndTurn,
        EndCombat
    }

    public class CombatManager : MonoBehaviour, ISubscriber
    {
        //private const string PlayerEndTurn = GlobalHelper.PlayerEndTurn;
        //private const string AiEndTurn = GlobalHelper.AiEndTurn;
        private const string EndTurn = GlobalHelper.EndTurn;
        private const string CombatFinished = GlobalHelper.CombatFinished;
        private const string EntityDead = GlobalHelper.EntityDead;

        private CombatState _currentCombatState;
        private Queue<Entity> _turnOrder;
        private Entity _activeEntity;
        private CombatMap _map;
        private GameObject _pawnHighlighterInstance;

        public List<Entity> Enemies; //todo refactor

        public GameObject PrototypePawnHighlighterPrefab;

        public static CombatManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            _currentCombatState = CombatState.Loading;
        }

        private void Update()
        {
            switch (_currentCombatState)
            {
                case CombatState.Loading: //we want to wait until we have enemy combatants populated

                    if (Enemies != null && Enemies.Count > 0)
                    {
                        _currentCombatState = CombatState.Start;
                    }

                    break;
                case CombatState.Start:
                    var party = TravelManager.Instance.Party.GetCompanions();

                    var combatants = new List<Entity>();

                    combatants.AddRange(party);

                    combatants.AddRange(Enemies);

                    _map = GenerateMap(combatants);

                    //todo draw map

                    _turnOrder = DetermineTurnOrder(combatants);

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

                    EventMediator.Instance.SubscribeToEvent(EndTurn, this);
                    break;
                case CombatState.AiTurn:
                    EventMediator.Instance.SubscribeToEvent(EndTurn, this);

                    //todo tell ai to do its thing

                    break;
                case CombatState.EndTurn:
                    RemoveDeadEntitiesFromTurnOrderDisplay();

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

        private GameObject GetPawnHighlighterInstance()
        {
            if (_pawnHighlighterInstance == null)
            {
                _pawnHighlighterInstance = Instantiate(PrototypePawnHighlighterPrefab, Vector3.zero, Quaternion.identity);
            }

            return _pawnHighlighterInstance;
        }

        private bool PlayerDead()
        {
            foreach (var entity in _turnOrder.ToArray())
            {
                if (entity.IsDead())
                {
                    continue;
                }

                if (entity.IsPlayer())
                {
                    return false;
                }
            }

            return true;
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

            while (_turnOrder.Peek().IsDead())
            {
                _turnOrder.Dequeue();
            }

            return _turnOrder.Peek();
        }

        private bool ActiveEntityPlayerControlled()
        {
            return _activeEntity.IsPlayer();
        }

        private void HighlightActiveEntitySprite()
        {
            var highlighter = GetPawnHighlighterInstance();

            highlighter.transform.position = _activeEntity.SpriteInstance.transform.position;
        }

        private void UpdateActiveEntityInfoPanel()
        {
            //todo might have panel subscribe to end turn event
        }

        //todo we can remove the dead entity's portrait from the turn order and remove it from queue when their turn comes around
        private void RemoveDeadEntitiesFromTurnOrderDisplay()
        {
            foreach (var entity in _turnOrder.ToArray())
            {
                if (entity.IsDead())
                {
                    //broadcast to remove its portrait and sprite from play.
                    //we'll probably do this when they are killed anyways so this is just a cleanup
                    EventMediator.Instance.Broadcast(EntityDead, this, entity);
                }
            }
        }

        private bool IsCombatFinished()
        {
            var playerEntities = false;
            var aiEntities = false;

            foreach (var entity in _turnOrder.ToArray())
            {
                if (entity.IsDead())
                {
                    continue;
                }

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

        private void DisplayPostCombatPopup()
        {
            EventMediator.Instance.Broadcast(CombatFinished, this);
        }

        private CombatMap GenerateMap(List<Entity> combatants)
        {
            return MapGenerator.Instance.Generate(combatants);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(EndTurn))
            {
                EventMediator.Instance.UnsubscribeFromEvent(EndTurn, this);

                _currentCombatState = CombatState.EndTurn;
            }
        }
    }
}
