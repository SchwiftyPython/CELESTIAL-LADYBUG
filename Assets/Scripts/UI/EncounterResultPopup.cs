using System.Collections.Generic;
using Assets.Scripts.Encounters;
using Assets.Scripts.Utilities.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EncounterResultPopup : MonoBehaviour, ISubscriber
    {
        private const string PopupEvent = GlobalHelper.EncounterResult;
        private const string EncounterFinished = GlobalHelper.EncounterFinished;
        private const string CampingEncounterFinished = GlobalHelper.CampingEncounterFinished;

        private TextWriter _textWriter;
        private EncounterType _encounterType;
        private bool _countsAsDayTraveled;

        public TextMeshProUGUI EncounterTitle;
        public TextMeshProUGUI ResultDescription;

        public GameObject OkayButton;

        [FMODUnity.EventRef] public string popupSound;

        private void Awake()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(PopupEvent, this);
            eventMediator.SubscribeToEvent(GlobalHelper.WritingFinished, this);

            _textWriter = GetComponent<TextWriter>();

            GetComponent<Button_UI>().ClickFunc = _textWriter.DisplayMessageInstantly;

            Hide();
        }

        private void Show(Encounter encounter, List<string> result)
        {
            HideButtons();

            _encounterType = encounter.EncounterType;

            if (_encounterType == EncounterType.Camping)
            {
                _countsAsDayTraveled = encounter.CountsAsDayTraveled;
            }

            EncounterTitle.text = encounter.Title;

            var resultText = string.Empty;
            foreach (var line in result)
            {
                resultText += '\n' + line;
            }

            _textWriter.AddWriter(ResultDescription, resultText, GlobalHelper.DefaultTextSpeed, true);

            gameObject.SetActive(true);

            GameManager.Instance.AddActiveWindow(gameObject);

            var sound = FMODUnity.RuntimeManager.CreateInstance(popupSound);
            sound.start();
        }

        private void ShowButtons()
        {
            OkayButton.SetActive(true);
        }

        private void HideButtons()
        {
            OkayButton.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);

            var eventMediator = FindObjectOfType<EventMediator>();

            if (_encounterType == EncounterType.Camping)
            {
                eventMediator.Broadcast(CampingEncounterFinished, this, _countsAsDayTraveled);
            }

            eventMediator.Broadcast(EncounterFinished, this);
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

                Show(encounter, result);
            }
            else if (eventName.Equals(GlobalHelper.WritingFinished))
            {
                ShowButtons();
            }
        }
    }
}
