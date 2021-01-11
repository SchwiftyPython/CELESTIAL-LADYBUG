﻿using System.Collections.Generic;
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

        public TextMeshProUGUI EncounterTitle;
        public TextMeshProUGUI ResultDescription;

        private void Awake()
        {
            EventMediator.Instance.SubscribeToEvent(PopupEvent, this);
            Hide();
        }

        private void Show(Encounter encounter, List<string> result)
        {
            _encounterType = encounter.EncounterType;

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

            EventMediator.Instance.Broadcast(EncounterFinished, this);

            if (_encounterType == EncounterType.Camping)
            {
                EventMediator.Instance.Broadcast(CampingEncounterFinished, this);
            }
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
