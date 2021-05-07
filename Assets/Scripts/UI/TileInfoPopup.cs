using Assets.Scripts.Combat;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    //todo need a Popup interface or base class
    public class TileInfoPopup : MonoBehaviour, ISubscriber
    {
        private const string HoverPopupEvent = GlobalHelper.TileHovered;
        private const string ClickPopupEvent = GlobalHelper.TileSelected;
        private const string HidePopupEvent = GlobalHelper.HidePopup;

        [SerializeField]
        private TextMeshProUGUI _tileType;

        [SerializeField]
        private TextMeshProUGUI _apCost;

        private void Start()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(HoverPopupEvent, this);
            eventMediator.SubscribeToEvent(ClickPopupEvent, this);
            Hide();
        }

        //todo refactor
        //for single tile ap cost on hover for some amount of time
        private void Show(Floor floorTile)
        {
            _tileType.text = floorTile.TileType.ToString();
            _apCost.text = $"{floorTile.ApCost} AP to cross";

            var position = Input.mousePosition;
            gameObject.transform.position = new Vector2(position.x + 120f, position.y + 110f);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(HidePopupEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        //todo refactor
        //for when tile selected and path is drawn
        private void Show(Floor floorTile, int apCostToTile)
        {
            _tileType.text = floorTile.TileType.ToString();
            _apCost.text = $"{apCostToTile} AP to move here";

            var position = Input.mousePosition;
            gameObject.transform.position = new Vector2(position.x + 90f, position.y + 80f);

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(HidePopupEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        private void Hide()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.UnsubscribeFromEvent(HidePopupEvent, this);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private void OnDestroy()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator?.UnsubscribeFromEvent(HoverPopupEvent, this);
            eventMediator?.UnsubscribeFromEvent(ClickPopupEvent, this);
            eventMediator?.UnsubscribeFromEvent(HidePopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(HoverPopupEvent))
            {
                if (!(broadcaster is Floor floorTile))
                {
                    return;
                }

                Show(floorTile);
            }
            else if(eventName.Equals(ClickPopupEvent))
            {
                if (!(broadcaster is Floor floorTile))
                {
                    return;
                }

                if (!(parameter is int totalApCost))
                {
                    return;
                }

                Show(floorTile, totalApCost);
            }
            else if(eventName.Equals(HidePopupEvent))
            {
                Hide();
            }
        }
    }
}
