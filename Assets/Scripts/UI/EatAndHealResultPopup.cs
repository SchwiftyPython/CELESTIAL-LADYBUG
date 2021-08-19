using System.Collections.Generic;
using Assets.Scripts.Utilities.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    //todo this is just placeholder for scrolling message console. Too clunky with this many popups
    public class EatAndHealResultPopup : MonoBehaviour, ISubscriber
    {
        private const string PopupEvent = GlobalHelper.PartyEatAndHeal;

        private TextWriter _textWriter;

        public TextMeshProUGUI ResultDescription;

        private void Awake()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(PopupEvent, this);

            _textWriter = GetComponent<TextWriter>();

            GetComponent<Button_UI>().ClickFunc = _textWriter.DisplayMessageInstantly;

            Hide();
        }

        public void Show(List<string> result)
        {
            var resultText = string.Empty;
            foreach (var line in result)
            {
                resultText += '\n' + line;
            }

            _textWriter.AddWriter(ResultDescription, resultText, GlobalHelper.DefaultTextSpeed, true);

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
            var eventMediator = FindObjectOfType<EventMediator>();

            if (eventMediator == null)
            {
                return;
            }

            eventMediator.UnsubscribeFromEvent(PopupEvent, this);
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
