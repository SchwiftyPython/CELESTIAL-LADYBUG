using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using GoRogue;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        EndCombat,
        NotActive
    }

    public class CombatManager : MonoBehaviour, ISubscriber
    {
        private const string EndTurnEvent = GlobalHelper.EndTurn;
        private const string CombatFinished = GlobalHelper.CombatFinished;
        private const string EntityDead = GlobalHelper.EntityDead;
        private const string RefreshUi = GlobalHelper.RefreshCombatUi;

        private CombatState _currentCombatState;
        private GameObject _pawnHighlighterInstance;

        private EventMediator _eventMediator;
        private TravelManager _travelManager;
        private CombatInputController _combatInput;

        private List<EffectTrigger<EffectArgs>> _effectTriggers;

        public CombatMap Map { get; private set; }

        public Entity ActiveEntity { get; private set; }

        public Queue<Entity> TurnOrder { get; private set; }

        public int CurrentTurnNumber { get; private set; }

        public List<Entity> Enemies; //todo refactor

        public GameObject PrototypePawnHighlighterPrefab;

        private void Awake()
        {
            _currentCombatState = CombatState.NotActive;

            _eventMediator = FindObjectOfType<EventMediator>();

            _travelManager = FindObjectOfType<TravelManager>();

            _combatInput = FindObjectOfType<CombatInputController>();
        }

        private void Update()
        {
            switch (_currentCombatState)
            {
                case CombatState.Loading: //we want to wait until we have enemy combatants populated

                    //todo travel manager checks for testing in combat scene
                    if (_travelManager != null && _travelManager.Party != null && Enemies != null && Enemies.Count > 0)
                    {
                        _currentCombatState = CombatState.Start;
                    }

                    break;
                case CombatState.Start:
                    CurrentTurnNumber = 1;

                    var party = _travelManager.Party.GetCompanions();

                    var combatants = new List<Entity>();

                    foreach (var companion in party)
                    {
                        if (companion.IsDerpus())
                        {
                            continue;
                        }

                        combatants.Add(companion);
                    }

                    combatants.AddRange(Enemies);

                    Map = GenerateMap(combatants);

                    DrawMap();

                    foreach (var combatant in combatants)
                    {
                        foreach (var ability in combatant.Abilities.Values)
                        {
                            ability.SetupForCombat();
                        }
                    }

                    TurnOrder = DetermineTurnOrder(combatants);

                    ActiveEntity = TurnOrder.Peek();

                    ActiveEntity.RefillActionPoints();

                    HighlightActiveEntitySprite();

                    if (ActiveEntityPlayerControlled())
                    {
                        _currentCombatState = CombatState.PlayerTurn;
                    }
                    else
                    {
                        _currentCombatState = CombatState.AiTurn;
                    }

                    _eventMediator.SubscribeToEvent(EndTurnEvent, this);
                    _eventMediator.SubscribeToEvent(GlobalHelper.EntityDead, this);
                    _eventMediator.SubscribeToEvent(GlobalHelper.ActiveEntityMoved, this);
                    _eventMediator.Broadcast(GlobalHelper.CombatSceneLoaded, this, Map);
                    _eventMediator.Broadcast(RefreshUi, this, ActiveEntity);

                    _combatInput.HighlightMovementRange();

                    break;
                case CombatState.PlayerTurn:

                    _eventMediator.Broadcast(GlobalHelper.PlayerTurn, this);
                   
                    break;
                case CombatState.AiTurn:
                    _eventMediator.Broadcast(GlobalHelper.AiTurn, this);

                    var activeSprite = ActiveEntity.CombatSpriteInstance;

                    if (ReferenceEquals(activeSprite, null))
                    {
                        return;
                    }

                    ActiveEntity.CombatSpriteInstance.GetComponent<AiController>().TakeTurn();
                    break;
                case CombatState.EndTurn:
                    if (IsCombatFinished())
                    {
                        _currentCombatState = CombatState.EndCombat;
                    }
                    else
                    {
                        ActiveEntity = GetNextInTurnOrder();

                        ActiveEntity.RefillActionPoints();

                        HighlightActiveEntitySprite();

                        _combatInput.HighlightMovementRange();

                        if (ActiveEntityPlayerControlled())
                        {
                            _currentCombatState = CombatState.PlayerTurn;
                        }
                        else
                        {
                            _currentCombatState = CombatState.AiTurn;
                        }

                        CurrentTurnNumber++;

                        _eventMediator.Broadcast(RefreshUi, this, ActiveEntity);

                        ActiveEntity.TriggerEffects();
                    }
                    break;
                case CombatState.EndCombat:
                    _eventMediator.UnsubscribeFromAllEvents(this);

                    DisplayPostCombatPopup();

                    //todo maybe move this portion to the post combat popup
                    if (PlayerDead())
                    {
                        _eventMediator.Broadcast(GlobalHelper.GameOver, this);
                    }
                    else
                    {
                        foreach (var combatant in TurnOrder)
                        {
                            combatant.ResetOneUseCombatAbilities();
                        }

                        if (!SceneManager.GetSceneByName(GlobalHelper.TravelScene).isLoaded)
                        {
                            SceneManager.LoadScene(GlobalHelper.TravelScene);
                        }
                    }

                    _currentCombatState = CombatState.NotActive;

                    break;
                case CombatState.NotActive:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Load()
        {
            _currentCombatState = CombatState.Loading;
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

        private void RemoveDeadEntity(Entity deadEntity)
        {
            TurnOrder = new Queue<Entity>(TurnOrder.Where(entity => entity != deadEntity));

            Map.RemoveEntity(deadEntity);

            Destroy(deadEntity.CombatSpriteInstance);

            //todo remove effects that originate from player
        }

        private bool ActiveEntityPlayerControlled()
        {
            return ActiveEntity.IsPlayer();
        }

        private void HighlightActiveEntitySprite() 
        {
            if (ActiveEntity == null)
            {
                ActiveEntity = TurnOrder.Peek();
            }

            var activeTile = Map.GetTileAt(ActiveEntity.Position);

            if (activeTile == null)
            {
                return;
            }

            activeTile.SpriteInstance.GetComponent<TerrainSlotUi>().HighlightTileForActiveEntity();
        }

        private void UpdateActiveEntityInfoPanel()
        {
            //todo might have panel subscribe to end turn event
        }

        private void RemoveDeadEntitiesFromTurnOrderDisplay()
        {
            foreach (var entity in TurnOrder.ToArray())
            {
                if (entity.IsDead())
                {
                    //broadcast to remove its portrait and sprite from play.
                    //we'll probably do this when they are killed anyways so this is just a cleanup
                    _eventMediator.Broadcast(EntityDead, this, entity);
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
            _eventMediator.Broadcast(CombatFinished, this);
        }

        private CombatMap GenerateMap(List<Entity> combatants)
        {
            return MapGenerator.Instance.Generate(combatants);
        }

        private void DrawMap()
        {
            BoardHolder.Instance.Build(Map);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(EndTurnEvent))
            {
                _currentCombatState = CombatState.EndTurn;
            }
            else if (eventName.Equals(GlobalHelper.EntityDead))
            {
                if (!(broadcaster is Entity deadEntity))
                {
                    return;
                }

                RemoveDeadEntity(deadEntity);

                if (IsCombatFinished())
                {
                    _currentCombatState = CombatState.EndCombat;
                }
            }
            else if (eventName.Equals(GlobalHelper.ActiveEntityMoved))
            {
                HighlightActiveEntitySprite();
                _combatInput.HighlightMovementRange();
            }
        }
    }
}
