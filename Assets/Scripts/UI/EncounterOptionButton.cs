using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EncounterOptionButton : MonoBehaviour
    {
        public void OptionSelected()
        {
            var optionText = transform.GetComponentInChildren<TextMeshProUGUI>().text;

            EventMediator.Instance.Broadcast(GlobalHelper.EncounterOptionSelected, this, optionText);
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
    }
}
