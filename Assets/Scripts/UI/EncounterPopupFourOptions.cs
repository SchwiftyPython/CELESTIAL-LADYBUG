using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Encounters;
using Assets.Scripts.Utilities.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EncounterPopupFourOptions : MonoBehaviour, ISubscriber
    {
        private const string PopupEvent = GlobalHelper.FourOptionEncounter;

        private Encounter _encounter;
        private TextWriter _textWriter;
        private TravelMessenger _travelMessenger;
        private List<GameObject> _optionButtons;

        public GameObject OptionButtonOne;
        public GameObject OptionButtonTwo;
        public GameObject OptionButtonThree;
        public GameObject OptionButtonFour;

        public TextMeshProUGUI EncounterTitle;
        public TextMeshProUGUI EncounterDescription;

        [FMODUnity.EventRef] public string popupSound;

        private void Awake()
        {
            _optionButtons = new List<GameObject>
            {
                OptionButtonOne,
                OptionButtonTwo,
                OptionButtonThree,
                OptionButtonFour
            };

            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(PopupEvent, this);
            eventMediator.SubscribeToEvent(GlobalHelper.WritingFinished, this);

            _textWriter = GetComponent<TextWriter>();
            _travelMessenger = FindObjectOfType<TravelMessenger>();

            GetComponent<Button_UI>().ClickFunc = _textWriter.DisplayMessageInstantly;

            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
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
            _travelMessenger.ClearMessageQueues();

            //todo might want to hide messages at this point too? 

            _encounter = encounter;

            EncounterTitle.text = _encounter.Title;

            DisableAllButtons();

            _textWriter.AddWriter(EncounterDescription, _encounter.Description, GlobalHelper.DefaultTextSpeed, true);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);

            var sound = FMODUnity.RuntimeManager.CreateInstance(popupSound);
            sound.start();
        }

        private void ShowButtons()
        {
            if (_encounter == null)
            {
                return;
            }

            var optionButtonIndex = 0;
            foreach (var optionText in _encounter.Options.Keys.ToArray())
            {
                var button = _optionButtons[optionButtonIndex].GetComponent<EncounterOptionButton>();

                button.SetOptionText(optionText);

                button.Show();

                optionButtonIndex++;
            }
        }

        public void Hide()
        {
            StartCoroutine(HideDelayed());
        }

        private IEnumerator HideDelayed()
        {
            yield return StartCoroutine(Delay());
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSecondsRealtime(.25f);
        }

        private void OnDestroy()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            if (eventMediator == null)
            {
                return;
            }

            eventMediator.UnsubscribeFromAllEvents(this);
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
            else if (eventName.Equals(GlobalHelper.WritingFinished))
            {
                ShowButtons();
            }
        }
    }
}
