using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            //todo just highlight the tile under the mouse

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
            //todo do this on left mouse click instead of hover

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

            foreach (var step in path.Steps)
            {
                var tileStep = _map.GetTerrain<Floor>(step);

                if (!_map.WalkabilityView[step])
                {
                    continue;
                }

                HighlightTile(tileStep);
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
