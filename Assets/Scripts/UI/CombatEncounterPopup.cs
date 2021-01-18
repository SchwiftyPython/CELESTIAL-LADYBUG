using System.Collections.Generic;
using Assets.Scripts.Encounters;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CombatEncounterPopup : MonoBehaviour, ISubscriber
    {
        private const string EncounterPopupEvent = GlobalHelper.CombatEncounter;
        private const string RetreatFailedPopupEvent = GlobalHelper.RetreatEncounterFailed;

        private List<GameObject> _optionButtons;

        public GameObject OptionButtonOne;
        public GameObject OptionButtonTwo;

        public TextMeshProUGUI EncounterTitle;
        public TextMeshProUGUI EncounterDescription;

        private void Awake()
        {
            _optionButtons = new List<GameObject>
            {
                OptionButtonOne,
                OptionButtonTwo
            };

            EventMediator.Instance.SubscribeToEvent(EncounterPopupEvent, this);
            EventMediator.Instance.SubscribeToEvent(RetreatFailedPopupEvent, this);
            Hide();
        }

        private void EnableAllButtons()
        {
            OptionButtonOne.SetActive(true);
            OptionButtonTwo.SetActive(true);
        }

        private void DisableAllButtons()
        {
            OptionButtonOne.SetActive(false);
            OptionButtonTwo.SetActive(false);
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

        private void ShowAfterRetreatFailed(Encounter encounter, List<string> result)
        {
            EncounterTitle.text = encounter.Title;

            var resultText = string.Empty;
            foreach (var line in result)
            {
                resultText += '\n' + line;
            }

            EncounterDescription.text = resultText;

            DisableAllButtons();

            var optionButtonIndex = 0;
            foreach (var optionText in encounter.Options.Keys)
            {
                var button = _optionButtons[optionButtonIndex].GetComponent<EncounterOptionButton>();

                button.SetOptionText(optionText);

                button.Show();

                //todo we want to make retreat not interactive for now -- refactor if clunky looking
                if (encounter.Options[optionText] is RetreatCombatOption)
                {
                    button.MakeNonInteractive();
                }

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
            EventMediator.Instance.UnsubscribeFromEvent(EncounterPopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(RetreatFailedPopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(EncounterPopupEvent))
            {
                var encounter = broadcaster as Encounter;

                if (encounter == null)
                {
                    return;
                }

                Show(encounter);
            }
            else if (eventName.Equals(RetreatFailedPopupEvent))
            {
                var encounter = broadcaster as Encounter;

                if (encounter == null)
                {
                    return;
                }

                var result = parameter as List<string>;

                if (result == null || result.Count < 1)
                {
                    return;
                }

                ShowAfterRetreatFailed(encounter, result);
            }
            
        }
    }
}
