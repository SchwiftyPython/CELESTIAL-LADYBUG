using Assets.Scripts.Combat;
using UnityEngine;

namespace Assets.Scripts.UI
{
    //todo can probably handle highlighting here too -- but don't fix it if it ain't broke
    public class OnMouseOverTile : MonoBehaviour, ISubscriber
    {
        private const string TileSelectedEvent = GlobalHelper.TileSelected;
        private const string TileDeselectedEvent = GlobalHelper.TileDeselected;

        private bool _tileSelected;

        public Tile Tile { get; set; }

        private void Start()
        {
            _tileSelected = false;

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(TileSelectedEvent, this);
            eventMediator.SubscribeToEvent(TileDeselectedEvent, this);
        }

        private void OnMouseEnter()
        {
            if (!_tileSelected)
            {
                var eventMediator = Object.FindObjectOfType<EventMediator>();
                eventMediator.Broadcast(GlobalHelper.TileHovered, Tile);
            }
        }

        private void OnMouseExit()
        {
            if (!_tileSelected)
            {
                var eventMediator = Object.FindObjectOfType<EventMediator>();
                eventMediator.Broadcast(GlobalHelper.HidePopup, this);
            }
        }

        private void OnMouseDown()
        {
            //todo maybe we can do cost here if combat input controller doesn't work out
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(TileSelectedEvent))
            {
                _tileSelected = true;
            }
            else if (eventName.Equals(TileDeselectedEvent))
            {
                _tileSelected = false;

                var eventMediator = Object.FindObjectOfType<EventMediator>();
                eventMediator.Broadcast(GlobalHelper.HidePopup, this);
            }
        }
    }
}
