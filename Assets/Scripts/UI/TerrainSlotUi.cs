using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TerrainSlotUi : MonoBehaviour, ITileHolder, IEntityHolder, ISubscriber
    {
        private Tile _tile;
        private SpriteRenderer _tileRenderer;
        private Entity _entity;

        private bool _isActiveEntityTile;

        private void Update()
        {
            if (!_isActiveEntityTile)
            {
                return;
            }

            _tileRenderer.color = Color.Lerp(Color.white, Color.gray, Mathf.PingPong(Time.time, 1));
        }

        public void HighlightTileForActiveEntity()
        {
            _isActiveEntityTile = true;

            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(GlobalHelper.ActiveEntityMoved, this);
            eventMediator.SubscribeToEvent(GlobalHelper.EndTurn, this);
        }

        public void RemoveActiveEntityHighlight()
        {
            _isActiveEntityTile = false;
            _tileRenderer.color = Color.white;

            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.UnsubscribeFromEvent(GlobalHelper.ActiveEntityMoved, this);
            eventMediator.UnsubscribeFromEvent(GlobalHelper.EndTurn, this);
        }

        public void SetTile(Tile tile)
        {
            _tile = tile;
            _tileRenderer = tile.SpriteInstance.GetComponent<SpriteRenderer>();
        }

        public void SetEntity(Entity entity)
        {
            _entity = entity;
        }

        public Tile GetTile()
        {
            return _tile;
        }

        public Entity GetEntity()
        {
            return _entity;
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.ActiveEntityMoved) || eventName.Equals(GlobalHelper.EndTurn))
            {
                RemoveActiveEntityHighlight();
            }
        }
    }
}
