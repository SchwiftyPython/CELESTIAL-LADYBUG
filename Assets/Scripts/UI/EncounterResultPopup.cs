using System.Collections.Generic;
using Assets.Scripts.Encounters;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EncounterResultPopup : MonoBehaviour, ISubscriber
    {
        private const string PopupEvent = GlobalHelper.EncounterResult;
        private const string EncounterFinished = GlobalHelper.EncounterFinished;
        private const string CampingEncounterFinished = GlobalHelper.CampingEncounterFinished;

        private EncounterType _encounterType;
        private bool _countsAsDayTraveled;

        public TextMeshProUGUI EncounterTitle;
        public TextMeshProUGUI ResultDescription;

        private void Awake()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(PopupEvent, this);
            Hide();
        }

        private void Show(Encounter encounter, List<string> result)
        {
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

            ResultDescription.text = resultText;

            gameObject.SetActive(true);

            GameManager.Instance.AddActiveWindow(gameObject);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            if (_encounterType == EncounterType.Camping)
            {
                eventMediator.Broadcast(CampingEncounterFinished, this, _countsAsDayTraveled);
            }

            eventMediator.Broadcast(EncounterFinished, this);
        }

        private void OnDestroy()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();

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
        }
    }
}
