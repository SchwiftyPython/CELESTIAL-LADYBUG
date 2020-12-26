using System.Collections.Generic;
using Assets.Scripts.Encounters;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EncounterPopupFourOptions : MonoBehaviour, ISubscriber
    {
        private const string PopupEvent = GlobalHelper.FourOptionEncounter;

        private List<GameObject> _optionButtons;

        public GameObject OptionButtonOne;
        public GameObject OptionButtonTwo;
        public GameObject OptionButtonThree;
        public GameObject OptionButtonFour;

        public TextMeshProUGUI EncounterTitle;
        public TextMeshProUGUI EncounterDescription;

        private void Awake()
        {
            _optionButtons = new List<GameObject>
            {
                OptionButtonOne,
                OptionButtonTwo,
                OptionButtonThree,
                OptionButtonFour
            };

            EventMediator.Instance.SubscribeToEvent(PopupEvent, this);
            Hide();
        }

        private void EnableAllButtons()
        {
            OptionButtonOne.SetActive(true);
            OptionButtonTwo.SetActive(true);
            OptionButtonThree.SetActive(true);
            OptionButtonFour.SetActive(true);
        }

        private void DisableAllButtons()
        {
            OptionButtonOne.SetActive(false);
            OptionButtonTwo.SetActive(false);
            OptionButtonThree.SetActive(false);
            OptionButtonFour.SetActive(false);
        }

        private void Show(Encounter encounter)
        {
            EncounterTitle.text = encounter.Title;
            EncounterDescription.text = encounter.Description;

            DisableAllButtons();

            var optionButtonIndex = 0;
            foreach (var optionText in encounter.Options.Keys)
            {
                var button = _optionButtons[optionButtonIndex].GetComponent<EncounterOptionButton>();

                button.SetOptionText(optionText);

                button.Show();

                optionButtonIndex++;
            }

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
                var encounter = broadcaster as Encounter;

                if (encounter == null)
                {
                    return;
                }

                Show(encounter);
            }
        }
    }
}
