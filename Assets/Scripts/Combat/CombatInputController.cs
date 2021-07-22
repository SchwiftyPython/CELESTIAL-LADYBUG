using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Abilities;
using Assets.Scripts.Entities;
using GoRogue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameObject = UnityEngine.GameObject;

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
        private List<Tile> _selectableTiles;

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

        public Color HighlightedColor;
        public Color MovementRangeColor;

        public GameObject Canvas;

        private void Start()
        {
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

        public void AbilityButtonClicked(Ability selectedAbility)
        {
            _selectedAbility = selectedAbility;
            _isAbilitySelected = true;
        }

        public bool AbilitySelected()
        {
            return _isAbilitySelected;
        }

        public bool TileSelected()
        {
            return _isTileSelected;
        }

        public Tile GetSelectedTile()
        {
            return _selectedTile;
        }

        public int GetTotalTileMovementCost()
        {
            return _apMovementCost;
        }

        public bool TargetInRange(Entity target)
        {
            return _selectedAbility.TargetInRange(target);
        }

        public bool TargetValid(Entity target)
        {
            return _selectedAbility.TargetValid(target);
        }

        public (int hitChance, List<string> positives, List<string> negatives) GetHitChance(Entity targetEntity)
        {
            IModifierProvider modifierProvider = _selectedAbility as IModifierProvider;

            _selectedAbilityTarget = targetEntity;

            if (_selectedAbility.IsRanged())
            {
                var rangedChance = _combatManager.ActiveEntity.CalculateChanceToHitRanged(_selectedAbilityTarget);

                int rangedMod = 0;
                if (modifierProvider != null)
                {
                    rangedMod = (int) modifierProvider.GetAdditiveModifiers(CombatModifierTypes.RangedToHit);
                }

                var rHitChance = rangedChance.hitChance + rangedMod;

                if (rHitChance <= 0)
                {
                    rHitChance = 1;
                }

                return (rHitChance, rangedChance.positives, rangedChance.negatives);
            }

            var meleeChance = _combatManager.ActiveEntity.CalculateChanceToHitMelee(_selectedAbilityTarget);

            int meleeMod = 0;
            if (modifierProvider != null)
            {
                meleeMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.MeleeToHit);
            }

            var mHitChance = meleeChance.hitChance + meleeMod;

            if (mHitChance <= 0)
            {
                mHitChance = 1;
            }

            return (mHitChance, meleeChance.positives, meleeChance.negatives);
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

        public void HighlightMovementRange() 
        {
            if (_selectableTiles != null && _selectableTiles.Count > 0)
            {
                foreach (var tile in _selectableTiles)
                {
                    tile.Visited = false;
                    tile.Selectable = false;
                    tile.TotalApCost = 0;

                    if (tile.SpriteInstance == null)
                    {
                        continue;
                    }

                    tile.SpriteInstance.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

            _selectableTiles = new List<Tile>();

            var activeEntity = _combatManager.ActiveEntity;

            if (!activeEntity.IsPlayer())
            {
                return;
            }

            var activeTile = _map.GetTileAt(activeEntity.Position);

            Queue<Tile> process = new Queue<Tile>();

            process.Enqueue(activeTile);
            activeTile.Visited = true;
            activeTile.TotalApCost = 0;

            while (process.Count > 0)
            {
                var currentTile = process.Dequeue();

                if (currentTile.TotalApCost < activeEntity.Stats.CurrentActionPoints)
                {
                    _selectableTiles.Add(currentTile);
                    currentTile.Selectable = true;

                    var adjacentTiles = currentTile.GetAdjacentTiles();

                    foreach (Tile tile in adjacentTiles)
                    {
                        var floor = tile as Floor;

                        if (floor == null || !floor.IsWalkable)
                        {
                            continue;
                        }

                        if (!floor.Visited)
                        {
                            //tile.parent = currentTile;
                            floor.Visited = true;
                            floor.TotalApCost = floor.ApCost + currentTile.TotalApCost;
                            process.Enqueue(floor);
                        }
                    }
                }
                else
                {
                    var path = _map.AStar.ShortestPath(activeEntity.Position, currentTile.Position);

                    if (path == null || !path.Steps.Any())
                    {
                        return;
                    }

                    var totalCost = 0;

                    foreach (var step in path.Steps)
                    {
                        var tileStep = _map.GetTerrain<Floor>(step);

                        if (!tileStep.IsWalkable)
                        {
                            continue;
                        }

                        totalCost += tileStep.ApCost;
                    }

                    if (totalCost <= activeEntity.Stats.CurrentActionPoints)
                    {
                        _selectableTiles.Add(currentTile);
                        currentTile.Selectable = true;

                        var adjacentTiles = currentTile.GetAdjacentTiles();

                        foreach (Tile tile in adjacentTiles)
                        {
                            var floor = tile as Floor;

                            if (floor == null || !floor.IsWalkable)
                            {
                                continue;
                            }

                            if (!floor.Visited)
                            {
                                floor.Visited = true;
                                floor.TotalApCost = floor.ApCost + currentTile.TotalApCost;
                                process.Enqueue(floor);
                            }
                        }
                    }
                }
            }

            foreach (var sTile in _selectableTiles)
            {
                sTile.SpriteInstance.GetComponent<SpriteRenderer>().color = MovementRangeColor;
            }
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

                if (tile.Selectable)
                {
                    tile.SpriteInstance.GetComponent<SpriteRenderer>().color = MovementRangeColor;
                }
                else
                {
                    tile.SpriteInstance.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }

            _highlightedTiles = new List<Tile>();
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
