using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    //todo this is just placeholder for scrolling message console. Too clunky with this many popups
    public class EatAndHealResultPopup : MonoBehaviour, ISubscriber
    {
        private const string PopupEvent = GlobalHelper.PartyEatAndHeal;

        public TextMeshProUGUI ResultDescription;

        private void Awake()
        {
            EventMediator.Instance.SubscribeToEvent(PopupEvent, this);
            Hide();
        }

        public void Show(List<string> result)
        {
            var resultText = string.Empty;
            foreach (var line in result)
            {
                resultText += '\n' + line;
            }

            ResultDescription.text = resultText;

            gameObject.SetActive(true);

            GameManager.Instance.AddActiveWindow(gameObject);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private void OnDestroy()
        {
            EventMediator.Instance.UnsubscribeFromEvent(PopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PopupEvent))
            {
                var result = parameter as List<string>;

                if (result == null || result.Count < 1)
                {
                    return;
                }

                Show(result);
            }
        }
    }
}
