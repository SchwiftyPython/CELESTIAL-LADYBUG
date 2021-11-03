using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI;
using Assets.Scripts.Audio;
using Assets.Scripts.Entities;
using Assets.Scripts.Saving;
using Assets.Scripts.Travel;
using Assets.Scripts.UI;
using GoRogue;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Combat
{
    public enum CombatState
    {
        LoadingScene,
        Start,
        PlayerTurn,
        AiTurn,
        EndTurn,
        EndCombat,
        NotActive,
        LoadFromSave
    }

    public enum CombatResult
    {
        Victory,
        Defeat,
        Retreat
    }

    public enum CombatStat
    {
        Kills,
        DamageDealt,
        DamageReceived
    }

    public class CompanionCombatStats
    {
        public int Kills;
        public int DamageDealt;
        public int DamageReceived;
    }

    public class CombatManager : MonoBehaviour, ISubscriber, ISaveable
    {
        private struct CombatManagerDto
        {
            public CombatState CurrentState;
            public object CombatMap;
            public string ActiveEntityId;
            public Queue<string> TurnOrder;
            public int TurnNumber;
            public List<object> Enemies;
            public Dictionary<string, CompanionCombatStats> CompanionIds;
            public Queue<string> CombatMessenger;
        }

        private const string EndTurnEvent = GlobalHelper.EndTurn;
        private const string CombatFinished = GlobalHelper.CombatFinished;
        private const string EntityDead = GlobalHelper.EntityDead;
        private const string RefreshUi = GlobalHelper.RefreshCombatUi;

        [SerializeField] private CombatState _currentCombatState;
        private GameObject _pawnHighlighterInstance;

        private EventMediator _eventMediator;
        private TravelManager _travelManager;
        private CombatInputController _combatInput;
        private MusicController _musicController;

        private List<EffectTrigger<EffectArgs>> _effectTriggers;

        [ES3NonSerializable] public CombatMap Map { get; private set; } //todo need to save manually somehow

        [ES3NonSerializable] public Entity ActiveEntity { get; private set; }

        public Queue<Entity> TurnOrder { get; private set; }

        public int CurrentTurnNumber { get; private set; }

        [ES3NonSerializable] public List<Entity> Enemies; //todo refactor
        [ES3NonSerializable] public Dictionary<Entity, CompanionCombatStats> Companions;

        public GameObject PrototypePawnHighlighterPrefab;

        private CombatResult _result;

        private void Awake()
        {
            _currentCombatState = CombatState.NotActive;

            _eventMediator = FindObjectOfType<EventMediator>();

            _travelManager = FindObjectOfType<TravelManager>();

            _combatInput = FindObjectOfType<CombatInputController>();

            _musicController = FindObjectOfType<MusicController>();
        }

        private void Update()
        {
            
            switch (_currentCombatState)
            {
                case CombatState.LoadingScene: //we want to wait until we have enemy combatants populated

                    _musicController.EndTravelMusic();

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
                    Companions = new Dictionary<Entity, CompanionCombatStats>();

                    foreach (var companion in party)
                    {
                        if (companion.IsDerpus())
                        {
                            continue;
                        }

                        combatants.Add(companion);
                        Companions.Add(companion, new CompanionCombatStats());
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

                    SubscribeToEvents();

                    _eventMediator.Broadcast(GlobalHelper.CombatSceneLoaded, this, Map);
                    _eventMediator.Broadcast(RefreshUi, this, ActiveEntity);

                    _musicController.PlayBattleMusic();

                    break;
                case CombatState.PlayerTurn:

                    _eventMediator.Broadcast(GlobalHelper.PlayerTurn, this);

                    var activePlayerSprite = ActiveEntity.CombatSpriteInstance;

                    if (activePlayerSprite == null)
                    {
                        RemoveEntity(ActiveEntity);
                        _currentCombatState = CombatState.EndTurn;
                        return;
                    }

                    var aiController = activePlayerSprite.GetComponent<AiController>();

                    if (ReferenceEquals(aiController, null))
                    {
                        return;
                    }

                    StartCoroutine(aiController.TakeTurn());
                    break;
                case CombatState.AiTurn:
                    _eventMediator.Broadcast(GlobalHelper.AiTurn, this);

                    var activeSprite = ActiveEntity.CombatSpriteInstance;

                    if (ReferenceEquals(activeSprite, null))
                    {
                        return;
                    }

                    AiTakeTurn();
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

                        ActiveEntity.UpdateMovedFlags();

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

                        _eventMediator.Broadcast(RefreshUi, this, ActiveEntity);

                        ActiveEntity.TriggerEffects();
                    }
                    break;
                case CombatState.EndCombat:
                    _eventMediator.UnsubscribeFromAllEvents(this);

                    if (_result != CombatResult.Retreat)
                    {
                        if (PlayerRetreated())
                        {
                            _result = CombatResult.Retreat;

                            //_musicController.PlayBattleVictoryMusic();

                            _musicController.EndBattleMusic();
                        }
                        else if (PlayerDead())
                        {
                            _result = CombatResult.Defeat;

                            _musicController.PlayBattleGameOverMusic();
                        }
                        else
                        {
                            _result = CombatResult.Victory;

                            //_musicController.PlayBattleVictoryMusic();

                            _musicController.EndBattleMusic();
                        }
                    }

                    DisplayPostCombatPopup(_result);

                    _currentCombatState = CombatState.NotActive;

                    break;
                case CombatState.NotActive:
                    break;
                case CombatState.LoadFromSave:
                    _eventMediator.UnsubscribeFromAllEvents(this);

                    SubscribeToEvents();

                    _eventMediator.Broadcast(GlobalHelper.CombatSceneLoaded, this, Map);
                    _eventMediator.Broadcast(RefreshUi, this, ActiveEntity);

                    _musicController.PlayBattleMusic();

                    _currentCombatState = CombatState.PlayerTurn;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateCombatStats(Entity companion, CombatStat stat, int value)
        {
            if (!Companions.ContainsKey(companion))
            {
                return;
            }

            switch (stat)
            {
                case CombatStat.Kills:
                    Companions[companion].Kills += value;
                    break;
                case CombatStat.DamageDealt:
                    Companions[companion].DamageDealt += value;
                    break;
                case CombatStat.DamageReceived:
                    Companions[companion].DamageReceived += value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
            }
        }

        public void LoadCombatScene()
        {
            _currentCombatState = CombatState.LoadingScene;
        }

        public void LoadFromSave()
        {
            _currentCombatState = CombatState.LoadFromSave;
        }

        public bool IsPlayerTurn()
        {
            return _currentCombatState == CombatState.PlayerTurn;
        }

        private void AiTakeTurn()
        {
            if (ActiveEntity.CombatSpriteInstance == null)
            {
                return;
            }

            var aiController = ActiveEntity.CombatSpriteInstance.GetComponent<AiController>();

            if (aiController == null)
            {
                return;
            }

            try
            {
                StartCoroutine(aiController.TakeTurn());
            }
            catch (Exception e)
            {
                Debug.Log("AI controller error");
                _eventMediator.Broadcast(EndTurnEvent, this);
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
            foreach (var entity in Companions.Keys.ToArray())
            {
                if (!entity.IsDead())
                {
                    return false;
                }
            }

            return true;
        }

        private bool PlayerRetreated()
        {
            if (PlayerDead())
            {
                return false;
            }

            if (TurnOrder.Any(e => e.IsPlayer()))
            {
                return false;
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

        public void RemoveEntity(Entity target)
        {
            TurnOrder = new Queue<Entity>(TurnOrder.Where(entity => entity != target));

            var currentTile = Map.GetTileAt(target.Position);

            var removed = Map.RemoveEntity(target);

            currentTile.SpriteInstance.GetComponent<TerrainSlotUi>().SetEntity(null);

            Destroy(target.CombatSpriteInstance);

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

            if (activeTile.SpriteInstance == null)
            {
                return;
            }

            var terrainSlotUi = activeTile.SpriteInstance.GetComponent<TerrainSlotUi>();

            if (terrainSlotUi == null)
            {
                return;
            }

            terrainSlotUi.HighlightTileForActiveEntity();
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

        public void Retreat()
        {
            foreach (var entity in Companions.Keys)
            {
                if (entity.IsDead())
                {
                    continue;
                }

                var entityInstance = entity.CombatSpriteInstance;

                entityInstance.AddComponent<AiController>();
                entityInstance.GetComponent<AiController>().SetSelf(entity);
                entityInstance.GetComponent<AiController>().Flee();
            }

            StartCoroutine(ActiveEntity.CombatSpriteInstance.GetComponent<AiController>().TakeTurn());
        }

        private void DisplayPostCombatPopup(CombatResult result)
        {
            var popup = FindObjectOfType<PostCombatResultsPopup>();

            popup.Show(result);

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

        private void SubscribeToEvents()
        {
            _eventMediator.SubscribeToEvent(EndTurnEvent, this);
            _eventMediator.SubscribeToEvent(GlobalHelper.EntityDead, this);
            _eventMediator.SubscribeToEvent(GlobalHelper.ActiveEntityMoved, this);
            _eventMediator.SubscribeToEvent(GlobalHelper.DamageDealt, this);
            _eventMediator.SubscribeToEvent(GlobalHelper.DamageReceived, this);
            _eventMediator.SubscribeToEvent(GlobalHelper.KilledTarget, this);
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

                if (IsCombatFinished())
                {
                    _currentCombatState = CombatState.EndCombat;
                }
            }
            else if (eventName.Equals(GlobalHelper.ActiveEntityMoved))
            {
                HighlightActiveEntitySprite();
            }
            else if (eventName.Equals(GlobalHelper.DamageDealt))
            {
                if (!(broadcaster is Entity entity))
                {
                    return;
                }

                if (!entity.IsPlayer())
                {
                    return;
                }

                if (!(parameter is int damage) || damage < 1)
                {
                    return;
                }

                UpdateCombatStats(entity, CombatStat.DamageDealt, damage);
            }
            else if (eventName.Equals(GlobalHelper.DamageReceived))
            {
                if (!(broadcaster is Entity entity))
                {
                    return;
                }

                if (!entity.IsPlayer())
                {
                    return;
                }

                if (!(parameter is int damage) || damage < 1)
                {
                    return;
                }

                UpdateCombatStats(entity, CombatStat.DamageReceived, damage);
            }
            else if(eventName.Equals(GlobalHelper.KilledTarget))
            {
                if (!(broadcaster is Entity entity))
                {
                    return;
                }

                if (!entity.IsPlayer())
                {
                    return;
                }

                UpdateCombatStats(entity, CombatStat.Kills, 1);
            }
        }

        public object CaptureState()
        {
            var dto = new CombatManagerDto
            {
                ActiveEntityId = ActiveEntity.Id,
                CombatMap = Map.CaptureState(),
                TurnNumber = CurrentTurnNumber,
                TurnOrder = new Queue<string>(),
                CurrentState = _currentCombatState,
                Enemies = new List<object>(),
                CompanionIds = new Dictionary<string, CompanionCombatStats>()
            };

            foreach (var companion in Companions)
            {
                dto.CompanionIds.Add(companion.Key.Id, companion.Value);
            }

            foreach (var enemy in Enemies)
            {
                dto.Enemies.Add(enemy.CaptureState());
            }

            foreach (var entity in TurnOrder)
            {
                dto.TurnOrder.Enqueue(entity.Id);
            }

            var messenger = FindObjectOfType<CombatMessenger>();

            dto.CombatMessenger = (Queue<string>)messenger.CaptureState();

            return dto;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }

            CombatManagerDto dto = (CombatManagerDto)state;

            Enemies = new List<Entity>();

            foreach (var enemy in dto.Enemies)
            {
                var restoredEnemy = new Entity();

                restoredEnemy.RestoreState(enemy);

                Enemies.Add(restoredEnemy);
            }

            ActiveEntity = _travelManager.Party.GetCompanionById(dto.ActiveEntityId);

            Companions = new Dictionary<Entity, CompanionCombatStats>();

            foreach (var id in dto.CompanionIds)
            {
                var companion = _travelManager.Party.GetCompanionById(id.Key);

                if (companion == null)
                {
                    Debug.LogError($"Can't find companion with id {id.Key}");
                    continue;
                }

                Companions.Add(companion, id.Value);
            }

            TurnOrder = new Queue<Entity>();

            foreach (var id in dto.TurnOrder)
            {
                Entity entity = null;

                foreach (var companion in Companions.Keys)
                {
                    if (string.Equals(id, companion.Id, StringComparison.OrdinalIgnoreCase))
                    {
                        entity = companion;
                        break;
                    }
                }

                if (entity == null)
                {
                    foreach (var enemy in Enemies)
                    {
                        if (string.Equals(id, enemy.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            entity = enemy;
                            break;
                        }
                    }
                }

                TurnOrder.Enqueue(entity);
            }

            Map = new CombatMap(MapGenerator.MapWidth, MapGenerator.MapHeight);

            Map.RestoreState(dto.CombatMap);

            _combatInput = FindObjectOfType<CombatInputController>();

            _combatInput.SetMap(Map);

            _combatInput.ClearHighlights();

            DrawMap();

            var messenger = FindObjectOfType<CombatMessenger>();

            messenger.RestoreState(dto.CombatMessenger);

            HighlightActiveEntitySprite();

            _eventMediator = FindObjectOfType<EventMediator>();

            _eventMediator.Broadcast(GlobalHelper.RefreshCombatUi, this, ActiveEntity);
        }
    }
}
