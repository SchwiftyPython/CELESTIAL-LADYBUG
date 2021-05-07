using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class EncounterOptionButton : MonoBehaviour
    {
        private Button _button;

        public void OptionSelected()
        {
            var optionText = transform.GetComponentInChildren<TextMeshProUGUI>().text;

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EncounterOptionSelected, this, optionText);
        }

        public void SetOptionText(string text)
        {
            transform.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void MakeInteractive()
        {
            if (_button == null)
            {
                _button = GetButtonComponent();
            }

            _button.interactable = true;
        }

        public void MakeNonInteractive()
        {
            if (_button == null)
            {
                _button = GetButtonComponent();
            }

            _button.interactable = true;
        }

        private Button GetButtonComponent()
        {
            return gameObject.GetComponent<Button>();
        }
    }
}
