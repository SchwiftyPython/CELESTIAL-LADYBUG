using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Combat
{
    public class CombatInputController : MonoBehaviour, ISubscriber
    {
        private const string CombatSceneLoaded = GlobalHelper.CombatSceneLoaded;
        private const string PlayerTurn = GlobalHelper.PlayerTurn;
        private const string AiTurn = GlobalHelper.AiTurn;
        private const string EndTurn = GlobalHelper.EndTurn;

        private bool _isPlayerTurn;
        private bool _isTileSelected;
        private bool _isAbilitySelected;

        private CombatMap _map;

        private List<Tile> _highlightedTiles;

        private Tile _selectedTile;

        private int _apMovementCost;

        private GraphicRaycaster _canvasGraphicRaycaster;
        private EventSystem _canvasEventSystem;

        private Queue<Entity> _abilityTargets;
        private Entity _selectedAbilityTarget;
        private Tile _highlightedAbilityTile;
        private Ability _selectedAbility;

        private EventMediator _eventMediator;
        private CombatManager _combatManager;

        public Color HighlightedColor; //todo refactor -- need to look into better way to highlight probably with an actual sprite

        public GameObject Canvas;

        //public static CombatInputController Instance;

        private void Start()
        {
            // if (Instance == null)
            // {
            //     Instance = this;
            // }
            // else if (Instance != this)
            // {
            //     Destroy(gameObject);
            // }

            if (Canvas == null)
            {
                Canvas = GameObject.Find("UI");
            }
            _canvasGraphicRaycaster = Canvas.GetComponent<GraphicRaycaster>();
            _canvasEventSystem = Canvas.GetComponent<EventSystem>();

            _eventMediator = Object.FindObjectOfType<EventMediator>();

            _eventMediator.SubscribeToEvent(CombatSceneLoaded, this);
            _eventMediator.SubscribeToEvent(PlayerTurn, this);
            _eventMediator.SubscribeToEvent(AiTurn, this);
            _eventMediator.SubscribeToEvent(EndTurn, this);

            _combatManager = FindObjectOfType<CombatManager>();
        }

        //todo need refactor big time
        private void Update()
        {
            if (_isPlayerTurn)
            {
                if (!_isTileSelected)
                {
                    if (MouseHitUi())
                    {
                        ClearHighlights();
                        return;
                    }

                    HighlightTileUnderMouse();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!_isTileSelected)
                    {
                        //check if ability is selected and if a valid target is clicked then use ability if true

                        if (_isAbilitySelected)
                        {
                            //todo need method for this block - Get <T> from mouse position - entity, tile, floor whatever

                            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                            var mouseCoord = new Coord((int)mousePosition.x, (int)mousePosition.y);

                            var entity = (Entity)_map.Entities.GetItems(mouseCoord).FirstOrDefault();

                            if (entity != null && _selectedAbility.TargetValid(entity) && _selectedAbility.TargetInRange(entity))
                            {
                                _selectedAbility.Use(entity);

                                _selectedAbility = null;

                                _isAbilitySelected = false;

                                ClearHighlights();

                                _eventMediator.Broadcast(GlobalHelper.RefreshCombatUi, this, _combatManager.ActiveEntity);
                            }
                        }
                        else
                        {
                            //todo only allow for tiles within entity movement range

                            //todo need to block when interacting with ui

                            if (MouseHitUi())
                            {
                                ClearHighlights();
                                return;
                            }

                            HighlightPathToMouse();
                        }
                    }
                    else
                    {
                        //check if same tile clicked then move there if true

                        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        var mouseCoord = new Coord((int)mousePosition.x, (int)mousePosition.y);

                        if (_map.OutOfBounds(mouseCoord))
                        {
                            return;
                        }

                        var targetTile = _map.GetTerrain<Floor>(mouseCoord);

                        if (targetTile == null || !targetTile.IsWalkable)
                        {
                            return;
                        }

                        if (ReferenceEquals(_selectedTile, targetTile))
                        {
                            var activeEntity = _combatManager.ActiveEntity;

                            if (_apMovementCost > activeEntity.Stats.CurrentActionPoints)
                            {
                                return;
                            }

                            activeEntity.MoveTo(targetTile, _apMovementCost);

                            _eventMediator.Broadcast(GlobalHelper.RefreshCombatUi, this, activeEntity);
                        }

                        _isTileSelected = false;

                        _eventMediator.Broadcast(GlobalHelper.TileDeselected, this);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    _isTileSelected = false;
                    _isAbilitySelected = false;

                    _eventMediator.Broadcast(GlobalHelper.TileDeselected, this);
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //todo clear if anything selected
                    //todo show menu if nothing selected

                    _isTileSelected = false;
                    _isAbilitySelected = false;

                    _eventMediator.Broadcast(GlobalHelper.TileDeselected, this);
                }
            }
        }

        public void AbilityButtonClicked(Queue<Entity> targets)
        {
            _abilityTargets = new Queue<Entity>(targets);

            _selectedAbilityTarget = targets.Peek();

            ClearHighlightUnderAbilityTarget();

            HighlightTileUnderAbilityTarget(_selectedAbilityTarget);

            var hitChance = _combatManager.ActiveEntity.CalculateChanceToHitMelee(_selectedAbilityTarget);

            //todo we'll need some kind of DTO to hold the hit chance and modifiers
            _eventMediator.Broadcast(GlobalHelper.EntityTargeted, _selectedAbilityTarget, hitChance);
        }

        public void AbilityButtonClicked(Ability selectedAbility)
        {
            _selectedAbility = selectedAbility;
            _isAbilitySelected = true;
        }

        public bool AbilitySelected()
        {
            return _isAbilitySelected;
        }

        public bool TargetInRange(Entity target)
        {
            return _selectedAbility.TargetInRange(target);
        }

        public bool TargetValid(Entity target)
        {
            return _selectedAbility.TargetValid(target);
        }

        public int GetHitChance(Entity targetEntity)
        {
            _selectedAbilityTarget = targetEntity;

            if (_selectedAbility.IsRanged())
            {
                return _combatManager.ActiveEntity.CalculateChanceToHitRanged(_selectedAbilityTarget);
            }

            return _combatManager.ActiveEntity.CalculateChanceToHitMelee(_selectedAbilityTarget);
        }

        private void NextTarget()
        {
            if (_abilityTargets == null || _abilityTargets.Count < 2)
            {
                return;
            }

            _eventMediator.Broadcast(GlobalHelper.HidePopup, this);

            ClearHighlightUnderAbilityTarget();

            var lastTarget = _abilityTargets.Dequeue();

            _abilityTargets.Enqueue(lastTarget);

            _selectedAbilityTarget = _abilityTargets.Peek();

            HighlightTileUnderAbilityTarget(_selectedAbilityTarget);

            var hitChance = _combatManager.ActiveEntity.CalculateChanceToHitMelee(_selectedAbilityTarget);

            //todo we'll need some kind of DTO to hold the hit chance and modifiers
            _eventMediator.Broadcast(GlobalHelper.EntityTargeted, _selectedAbilityTarget, hitChance);
        }

        private bool MouseHitUi()
        {
            if (Canvas == null)
            {
                Canvas = GameObject.Find("UI");
            }
            _canvasGraphicRaycaster = Canvas.GetComponent<GraphicRaycaster>();
            _canvasEventSystem = Canvas.GetComponent<EventSystem>();

            var pointerEventData = new PointerEventData(_canvasEventSystem) { position = Input.mousePosition };

            var results = new List<RaycastResult>();

            _canvasGraphicRaycaster.Raycast(pointerEventData, results);

            return results.Any();
        }

        private void HighlightTileUnderMouse()
        {
            ClearHighlights();

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var mouseCoord = new Coord((int)mousePosition.x, (int)mousePosition.y);

            if (_map.OutOfBounds(mouseCoord))
            {
                return;
            }

            var targetTile = _map.GetTerrain<Floor>(mouseCoord);

            if (targetTile == null || !targetTile.IsWalkable)
            {
                return;
            }

            HighlightTile(targetTile);
        }

        private void HighlightPathToMouse()
        {
            ClearHighlights();

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var mouseCoord = new Coord((int) mousePosition.x, (int) mousePosition.y);

            if (_map.OutOfBounds(mouseCoord))
            {
                _isTileSelected = false;
                return;
            }

            var targetTile = _map.GetTerrain<Floor>(mouseCoord);

            if (targetTile == null || !targetTile.IsWalkable || !_map.WalkabilityView[mouseCoord])
            {
                _isTileSelected = false;
                return;
            }

            HighlightPathToTarget(targetTile);
        }

        private void HighlightPathToTarget(Tile tile)
        {
            if (_highlightedTiles == null)
            {
                _highlightedTiles = new List<Tile>();
            }

            var activeEntity = _combatManager.ActiveEntity;

            var path = _map.AStar.ShortestPath(activeEntity.Position, tile.Position);

            if (path == null || !path.Steps.Any())
            {
                _isTileSelected = false;
                return;
            }

            _selectedTile = tile;

            _apMovementCost = 0;

            //todo not sure if first and last steps are inclusive - don't want to count first tile if it is the start point
            foreach (var step in path.Steps)
            {
                var tileStep = _map.GetTerrain<Floor>(step);

                if (!_map.WalkabilityView[step])
                {
                    //todo does this ever happen?
                    continue;
                }

                _apMovementCost += tileStep.ApCost;

                HighlightTile(tileStep);
            }

            _isTileSelected = true;

            _eventMediator.Broadcast(GlobalHelper.HidePopup, this);

            _eventMediator.Broadcast(GlobalHelper.TileSelected, tile, _apMovementCost);
        }

        private void HighlightTile(Tile tile)
        {
            if (_highlightedTiles == null)
            {
                _highlightedTiles = new List<Tile>();
            }

            _highlightedTiles.Add(tile);
            tile.SpriteInstance.GetComponent<SpriteRenderer>().color = HighlightedColor;
        }

        private void HighlightTileUnderAbilityTarget(Entity abilityTarget)
        {
            if (abilityTarget == null)
            {
                return;
            }

            _highlightedAbilityTile = abilityTarget.CurrentMap.GetTerrain<Tile>(abilityTarget.Position);
            _highlightedAbilityTile.SpriteInstance.GetComponent<SpriteRenderer>().color = HighlightedColor;
        }

        private void ClearHighlightUnderAbilityTarget()
        {
            if (_highlightedAbilityTile == null)
            {
                return;
            }

            _highlightedAbilityTile.SpriteInstance.GetComponent<SpriteRenderer>().color = Color.white;

            _highlightedAbilityTile = null;
        }

        public void ClearHighlights()
        {
            if (_highlightedTiles == null || _highlightedTiles.Count <= 0)
            {
                return;
            }

            foreach (var tile in _highlightedTiles)
            {
                if (tile.SpriteInstance == null)
                {
                    continue;
                }

                tile.SpriteInstance.GetComponent<SpriteRenderer>().color = Color.white;
            }

            _highlightedTiles = new List<Tile>();
        }

        private void ShowEntityInfo(Entity targetEntity)
        {
            _eventMediator.Broadcast(GlobalHelper.HidePopup, this);

            _eventMediator.Broadcast(GlobalHelper.TileHovered, this, targetEntity);
        }

        private void ShowHitChance(Entity targetEntity)
        {
            _eventMediator.Broadcast(GlobalHelper.HidePopup, this);

            _selectedAbilityTarget = targetEntity;

            ClearHighlightUnderAbilityTarget();

            HighlightTileUnderAbilityTarget(_selectedAbilityTarget);

            var hitChance = _combatManager.ActiveEntity.CalculateChanceToHitMelee(_selectedAbilityTarget);

            //todo we'll need some kind of DTO to hold the hit chance and modifiers
            _eventMediator.Broadcast(GlobalHelper.EntityTargeted, _selectedAbilityTarget, hitChance);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(CombatSceneLoaded))
            {
                _map = parameter as CombatMap;
            }
            else if (eventName.Equals(PlayerTurn))
            {
                _isPlayerTurn = true;
            }
            else if (eventName.Equals(AiTurn))
            {
                _isPlayerTurn = false;
            }
            else if (eventName.Equals(EndTurn))
            {
                ClearHighlightUnderAbilityTarget();
            }
        }
    }
}
