using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EntityInfoPopup : MonoBehaviour, ISubscriber
    {
        private const string HoverPopupEvent = GlobalHelper.TileHovered;
        private const string HidePopupEvent = GlobalHelper.HidePopup;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _health;

        [SerializeField]
        private TextMeshProUGUI _nextTurn;

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(HoverPopupEvent, this);
            Hide();
        }

        private void Show(Entity targetEntity)
        {
            _name.text = targetEntity.Name;
            _health.text = $"{targetEntity.Stats.CurrentHealth}/{targetEntity.Stats.MaxHealth}";

            var nextTurn = CombatManager.Instance.TurnOrder.ToArray().ToList().IndexOf(targetEntity);

            _nextTurn.text = $"Acts in {nextTurn} turns";

            var position = Input.mousePosition;
            gameObject.transform.position = new Vector2(position.x + 90f, position.y + 80f);

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
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(HoverPopupEvent))
            {
                if (!(parameter is Entity targetEntity))
                {
                    return;
                }

                Show(targetEntity);
            }
            else if (eventName.Equals(HidePopupEvent))
            {
                Hide();
            }
        }
    }
}
