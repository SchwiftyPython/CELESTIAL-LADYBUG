using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class EndTurnButton : MonoBehaviour, ISubscriber
    {
        private const string EnableButtonEvent = GlobalHelper.PlayerTurn;
        private const string DisableButtonEvent = GlobalHelper.AiTurn;

        private Button _button;

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(EnableButtonEvent, this);
            eventMediator.SubscribeToEvent(DisableButtonEvent, this);

            _button = GetComponent<Button>();
        }

        public void OnClick()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EndTurn, this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (string.Equals(EnableButtonEvent, eventName, StringComparison.OrdinalIgnoreCase))
            {
                _button.interactable = true;
            }
            else if (string.Equals(DisableButtonEvent, eventName, StringComparison.OrdinalIgnoreCase))
            {
                _button.interactable = false;
            }
        }
    }
}
