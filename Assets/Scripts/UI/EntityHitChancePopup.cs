using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EntityHitChancePopup : MonoBehaviour, ISubscriber
    {
        private const string PopupEvent = GlobalHelper.EntityTargeted;
        private const string HideEvent = GlobalHelper.HidePopup;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _health;

        [SerializeField]
        private TextMeshProUGUI _hitChance;

        [SerializeField]
        private TextMeshProUGUI _nextTurn;

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(PopupEvent, this);
            Hide();
        }

        private void Show(Entity targetEntity, int hitChance)
        {
            //todo should show equipped armor toughness

            _name.text = targetEntity.Name;
            _health.text = $"{targetEntity.Stats.CurrentHealth}/{targetEntity.Stats.MaxHealth}";

            //todo modifiers
            _hitChance.text = $"{hitChance}% Chance to hit";

            var nextTurn = CombatManager.Instance.TurnOrder.ToList().IndexOf(targetEntity);

            _nextTurn.text = $"Acts in {nextTurn} turns";

            var position = Camera.main.WorldToScreenPoint(targetEntity.CombatSpriteInstance.transform.position);
            gameObject.transform.position = new Vector2(position.x + 150f, position.y - 75f);

            EventMediator.Instance.SubscribeToEvent(HideEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        private void Hide()
        {
            EventMediator.Instance.UnsubscribeFromEvent(HideEvent, this);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private void OnDestroy()
        {
            EventMediator.Instance.UnsubscribeFromEvent(PopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(HideEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PopupEvent))
            {
                if (!(broadcaster is Entity targetEntity))
                {
                    return;
                }

                if (!(parameter is int hitChance))
                {
                    return;
                }

                Show(targetEntity, hitChance);
            }
            else if (eventName.Equals(HideEvent))
            {
                //todo check this against a list of valid broadcasters
                if (broadcaster is UseAbilityButton)
                {
                    return;
                }

                Hide();
            }
        }
    }
}
