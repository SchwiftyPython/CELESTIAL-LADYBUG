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

        private void Show()
        {
            EventMediator.Instance.Broadcast(GlobalHelper.PopulateCharacterSheet, this, _companions.First());
            EventMediator.Instance.Broadcast(GlobalHelper.EquipmentUpdated, this, _companions.First());
            EventMediator.Instance.Broadcast(GlobalHelper.PauseTimer, this);

            EventMediator.Instance.SubscribeToEvent(HidePopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(ShowPopupEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        private void Hide()
        {
            EventMediator.Instance.SubscribeToEvent(ShowPopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
            EventMediator.Instance.Broadcast(GlobalHelper.ResumeTimer, this);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }
        
        private void OnDestroy()
        {
            EventMediator.Instance.UnsubscribeFromEvent(ShowPopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
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

            EventMediator.Instance.Broadcast(GlobalHelper.PopulateCharacterSheet, this, _companions[_currentIndex]);
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

            EventMediator.Instance.Broadcast(GlobalHelper.PopulateCharacterSheet, this, _companions[_currentIndex]);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(ShowPopupEvent))
            {
                _companions = TravelManager.Instance.Party.GetCompanions();
                Show();
            }
            else if (eventName.Equals(HidePopupEvent))
            {
                Hide();
            }
        }
    }
}
