using System.Collections.Generic;
using System.Linq;
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

        private CombatMap _map;

        private List<Tile> _highlightedTiles;

        private Tile _selectedTile;

        private int _apMovementCost;

        private GraphicRaycaster _canvasGraphicRaycaster;
        private EventSystem _canvasEventSystem;

        private Queue<Entity> _abilityTargets;
        private Entity _selectedAbilityTarget;
        private Tile _highlightedAbilityTile;

        public Color HighlightedColor; //todo refactor -- need to look into better way to highlight probably with an actual sprite

        public GameObject Canvas;

        public static CombatInputController Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            if (Canvas == null)
            {
                Canvas = GameObject.Find("UI");
            }
            _canvasGraphicRaycaster = Canvas.GetComponent<GraphicRaycaster>();
            _canvasEventSystem = Canvas.GetComponent<EventSystem>();

            EventMediator.Instance.SubscribeToEvent(CombatSceneLoaded, this);
            EventMediator.Instance.SubscribeToEvent(PlayerTurn, this);
            EventMediator.Instance.SubscribeToEvent(AiTurn, this);
            EventMediator.Instance.SubscribeToEvent(EndTurn, this);
        }

        //todo need refactor big time
        private void Update()
        {
            if (_isPlayerTurn)
            {
                if (!_isTileSelected)
                {
                    //todo highlight tile or show entity info if entity present

                    if (MouseHitUi())
                    {
                        ClearHighlights();
                        return;
                    }

                    var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    var mouseCoord = new Coord((int)mousePosition.x, (int)mousePosition.y);

                    var entity = (Entity) _map.Entities.GetItems(mouseCoord).FirstOrDefault();

                    if (entity == null)
                    {
                        HighlightTileUnderMouse();
                    }
                    else
                    {
                        ShowEntityInfo(entity);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!_isTileSelected)
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

                        if (_selectedTile == targetTile)
                        {
                            var activeEntity = CombatManager.Instance.ActiveEntity;

                            if (_apMovementCost > activeEntity.Stats.CurrentActionPoints)
                            {
                                return;
                            }

                            activeEntity.MoveTo(targetTile, _apMovementCost);

                            EventMediator.Instance.Broadcast(GlobalHelper.RefreshCombatUi, this, activeEntity);
                        }

                        _isTileSelected = false;

                        EventMediator.Instance.Broadcast(GlobalHelper.TileDeselected, this);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    _isTileSelected = false;

                    EventMediator.Instance.Broadcast(GlobalHelper.TileDeselected, this);
                }
                else if (Input.GetKeyDown(KeyCode.Tab))
                {
                    //EventMediator.Instance.Broadcast(GlobalHelper.NextTarget, this);

                    NextTarget();
                }
            }
        }

        public void AbilityButtonClicked(Queue<Entity> targets)
        {
            _abilityTargets = new Queue<Entity>(targets);

            _selectedAbilityTarget = targets.Peek();

            ClearHighlightUnderAbilityTarget();

            HighlightTileUnderAbilityTarget(_selectedAbilityTarget);
        }

        private void NextTarget()
        {
            if (_abilityTargets == null || _abilityTargets.Count < 2)
            {
                return;
            }

            ClearHighlightUnderAbilityTarget();

            var lastTarget = _abilityTargets.Dequeue();

            _abilityTargets.Enqueue(lastTarget);

            _selectedAbilityTarget = _abilityTargets.Peek();

            HighlightTileUnderAbilityTarget(_selectedAbilityTarget);
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

            var activeEntity = CombatManager.Instance.ActiveEntity;

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

            EventMediator.Instance.Broadcast(GlobalHelper.HidePopup, this);

            EventMediator.Instance.Broadcast(GlobalHelper.TileSelected, tile, _apMovementCost);
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
            EventMediator.Instance.Broadcast(GlobalHelper.HidePopup, this);

            EventMediator.Instance.Broadcast(GlobalHelper.TileHovered, this, targetEntity);
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
