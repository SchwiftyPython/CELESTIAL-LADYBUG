using System.Collections.Generic;
using System.Linq;
using GoRogue;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class CombatInputController : MonoBehaviour, ISubscriber
    {
        private const string CombatSceneLoaded = GlobalHelper.CombatSceneLoaded;
        private const string PlayerTurn = GlobalHelper.PlayerTurn;
        private const string AiTurn = GlobalHelper.AiTurn;

        private bool _isPlayerTurn;
        private bool _tileSelected;

        private CombatMap _map;

        private List<Tile> _highlightedTiles;

        private int _apMovementCost;

        public Color HighlightedColor; //todo refactor -- need to look into better way to highlight probably with an actual sprite

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(CombatSceneLoaded, this);
            EventMediator.Instance.SubscribeToEvent(PlayerTurn, this);
            EventMediator.Instance.SubscribeToEvent(AiTurn, this);
        }

        private void Update()
        {
            if (_isPlayerTurn)
            {
                if (!_tileSelected)
                {
                    HighlightTileUnderMouse();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!_tileSelected)
                    {
                        _tileSelected = true;
                    }
                    else
                    {
                        _tileSelected = false;
                    }

                    HighlightPathToMouse();
                }
                else if (Input.GetMouseButtonDown(1))
                {

                }
            }
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
                return;
            }

            var targetTile = _map.GetTerrain<Floor>(mouseCoord);

            if (targetTile == null || !targetTile.IsWalkable)
            {
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
                return;
            }

            _apMovementCost = 0;

            //todo not sure if first and last steps are inclusive - don't want to count first tile if it is the start point
            foreach (var step in path.Steps)
            {
                var tileStep = _map.GetTerrain<Floor>(step);

                if (!_map.WalkabilityView[step])
                {
                    continue;
                }

                _apMovementCost += tileStep.ApCost;

                HighlightTile(tileStep);
            }

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
        }
    }
}
