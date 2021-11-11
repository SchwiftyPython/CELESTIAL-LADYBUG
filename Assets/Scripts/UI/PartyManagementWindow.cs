using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PartyManagementWindow : MonoBehaviour, ISubscriber
    {
        private const string ShowPopupEvent = GlobalHelper.ManageParty;
        private const string HidePopupEvent = GlobalHelper.HidePartyManagement;

        private List<Entity> _companions;

        private int _currentIndex;

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            var travelManager = Object.FindObjectOfType<TravelManager>();
            _companions = travelManager.Party.GetCompanions();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.PopulateCharacterSheet, this, _companions.First());
            eventMediator.Broadcast(GlobalHelper.EquipmentUpdated, this, _companions.First());
            eventMediator.Broadcast(GlobalHelper.PauseTimer, this);

            eventMediator.SubscribeToEvent(HidePopupEvent, this);
            eventMediator.UnsubscribeFromEvent(ShowPopupEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        public void Hide()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(ShowPopupEvent, this);
            eventMediator.UnsubscribeFromEvent(HidePopupEvent, this);
            eventMediator.Broadcast(GlobalHelper.ResumeTimer, this);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }
        
        private void OnDestroy()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();

            if (eventMediator == null)
            {
                return;
            }

            eventMediator.UnsubscribeFromEvent(ShowPopupEvent, this);
            eventMediator.UnsubscribeFromEvent(HidePopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void NextCompanion()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return;
            }

            _currentIndex++;

            if (_currentIndex >= _companions.Count)
            {
                _currentIndex = 0;
            }

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.PopulateCharacterSheet, this, _companions[_currentIndex]);

            var characterSheet = FindObjectOfType<CompanionCharacterSheet>();

            characterSheet.Populate(_companions[_currentIndex]);
        }

        public void PreviousCompanion()
        {
            if (_companions == null || _companions.Count < 1)
            {
                return;
            }

            _currentIndex--;

            if (_currentIndex < 0)
            {
                _currentIndex = _companions.Count - 1;
            }

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.PopulateCharacterSheet, this, _companions[_currentIndex]);

            var characterSheet = FindObjectOfType<CompanionCharacterSheet>();

            characterSheet.Populate(_companions[_currentIndex]);
        }

        public Entity GetCurrentCompanion()
        {
            return _companions[_currentIndex];
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(ShowPopupEvent))
            {
                Show();
            }
            else if (eventName.Equals(HidePopupEvent))
            {
                Hide();
            }
        }
    }
}
