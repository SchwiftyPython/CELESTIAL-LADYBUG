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
        private const string RefreshUi = GlobalHelper.RefreshCombatUi;

        private CombatState _currentCombatState;
        private CombatMap _map;
        private GameObject _pawnHighlighterInstance;

        public Entity ActiveEntity { get; private set; }

        public Queue<Entity> TurnOrder { get; private set; }

        public int CurrentTurnNumber { get; private set; }

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

                    //todo travel manager checks for testing in combat scene
                    if (TravelManager.Instance != null && TravelManager.Instance.Party != null && Enemies != null && Enemies.Count > 0)
                    {
                        _currentCombatState = CombatState.Start;
                    }

                    break;
                case CombatState.Start:
                    CurrentTurnNumber = 1;

                    var party = TravelManager.Instance.Party.GetCompanions();

                    var combatants = new List<Entity>();

                    combatants.AddRange(party);

                    combatants.AddRange(Enemies);

                    _map = GenerateMap(combatants);

                    DrawMap();

                    TurnOrder = DetermineTurnOrder(combatants);

                    ActiveEntity = TurnOrder.Peek();

                    EventMediator.Instance.Broadcast(RefreshUi, this, ActiveEntity);

                    HighlightActiveEntitySprite();

                    if (ActiveEntityPlayerControlled())
                    {
                        _currentCombatState = CombatState.PlayerTurn;
                    }
                    else
                    {
                        _currentCombatState = CombatState.AiTurn;
                    }

                    EventMediator.Instance.Broadcast(GlobalHelper.CombatSceneLoaded, this, _map);
                    break;
                case CombatState.PlayerTurn:
                    UpdateActiveEntityInfoPanel();

                    EventMediator.Instance.Broadcast(GlobalHelper.PlayerTurn, this);
                    EventMediator.Instance.SubscribeToEvent(EndTurn, this);
                    break;
                case CombatState.AiTurn:
                    EventMediator.Instance.Broadcast(GlobalHelper.AiTurn, this);
                    EventMediator.Instance.SubscribeToEvent(EndTurn, this);

                    //todo tell ai to do its thing

                    //todo skipping ai turn until implemented
                    EventMediator.Instance.Broadcast(EndTurn, this);

                    break;
                case CombatState.EndTurn:
                    if (IsCombatFinished())
                    {
                        _currentCombatState = CombatState.EndCombat;
                    }
                    else
                    {
                        ActiveEntity = GetNextInTurnOrder();

                        HighlightActiveEntitySprite();

                        if (ActiveEntityPlayerControlled())
                        {
                            _currentCombatState = CombatState.PlayerTurn;
                        }
                        else
                        {
                            _currentCombatState = CombatState.AiTurn;
                        }

                        CurrentTurnNumber++;

                        EventMediator.Instance.Broadcast(RefreshUi, this, ActiveEntity);
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
            foreach (var entity in TurnOrder.ToArray())
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
            var lastEntity = TurnOrder.Dequeue();

            TurnOrder.Enqueue(lastEntity);

            while (TurnOrder.Peek().IsDead())
            {
                TurnOrder.Dequeue();
            }

            return TurnOrder.Peek();
        }

        private bool ActiveEntityPlayerControlled()
        {
            return ActiveEntity.IsPlayer();
        }

        private void HighlightActiveEntitySprite()
        {
            var highlighter = GetPawnHighlighterInstance();

            highlighter.transform.position = ActiveEntity.CombatSpriteInstance.transform.position;
        }

        private void UpdateActiveEntityInfoPanel()
        {
            //todo might have panel subscribe to end turn event
        }

        //todo we can remove the dead entity's portrait from the turn order and remove it from queue when their turn comes around
        //this happens on refresh anyways
        private void RemoveDeadEntitiesFromTurnOrderDisplay()
        {
            foreach (var entity in TurnOrder.ToArray())
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

            foreach (var entity in TurnOrder.ToArray())
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

        private void DrawMap()
        {
            BoardHolder.Instance.Build(_map);
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
