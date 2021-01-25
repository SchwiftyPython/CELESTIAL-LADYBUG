using Assets.Scripts.Combat;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    //todo need a Popup interface
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
            EventMediator.Instance.SubscribeToEvent(HoverPopupEvent, this);
            EventMediator.Instance.SubscribeToEvent(ClickPopupEvent, this);
            EventMediator.Instance.SubscribeToEvent(HidePopupEvent, this);
            Hide();
        }

        //for single tile ap cost on hover for some amount of time
        private void Show(Floor floorTile)
        {
            _tileType.text = floorTile.TileType.ToString();
            _apCost.text = floorTile.ApCost.ToString();

            EventMediator.Instance.SubscribeToEvent(HidePopupEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        //for when tile selected and path is drawn
        private void Show(Floor floorTile, int apCostToTile)
        {
            _tileType.text = floorTile.TileType.ToString();
            _apCost.text = apCostToTile.ToString();

            EventMediator.Instance.SubscribeToEvent(HidePopupEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        private void Hide()
        {
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private void OnDestroy()
        {
            EventMediator.Instance.UnsubscribeFromEvent(HoverPopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(ClickPopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
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
