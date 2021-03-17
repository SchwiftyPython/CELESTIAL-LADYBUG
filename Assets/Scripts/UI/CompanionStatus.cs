using System.Collections.Generic;
using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CompanionStatus : MonoBehaviour, ISubscriber
    {
        private readonly List<string> _refreshEvents = new List<string>
        {
            GlobalHelper.EncounterFinished
        };

        private Entity _companion;

        public TextMeshProUGUI Name;
        public TextMeshProUGUI Health;
        public TextMeshProUGUI Energy;
        public TextMeshProUGUI Morale;

        public void Populate(Entity companion)
        {
            _companion = companion;

            Name.text = _companion.Name;
            Health.text = $"{_companion.Stats.CurrentHealth}/{_companion.Stats.MaxHealth}";
            Energy.text = $"{_companion.Stats.CurrentEnergy}/{_companion.Stats.MaxEnergy}";
            Morale.text = $"{_companion.Stats.CurrentMorale}/{_companion.Stats.MaxMorale}";
        }

        public void Refresh()
        {
            Populate(_companion);
        }

        public void Show()
        {
            Refresh();
            SubscribeToEvents();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            UnsubscribeFromEvents();
            gameObject.SetActive(false);
        }

        private void SubscribeToEvents()
        {
            foreach (var eventName in _refreshEvents)
            {
                EventMediator.Instance.SubscribeToEvent(eventName, this);
            }
        }

        private void UnsubscribeFromEvents()
        {
            EventMediator.Instance.UnsubscribeFromAllEvents(this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (_refreshEvents.Contains(eventName))
            {
                Refresh();
            }
        }
    }
}
