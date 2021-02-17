using System.Collections.Generic;
using Assets.Scripts.Travel;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TravelStatusBar : MonoBehaviour, ISubscriber
    {
        private readonly List<string> _refreshEvents = new List<string>
        {
            GlobalHelper.EncounterFinished
        };

        private const int MaxStatusesDisplayed = 4;

        private int _currentStartingIndex;

        public TextMeshProUGUI GoldValue;
        public TextMeshProUGUI FoodValue;
        public TextMeshProUGUI HealthPotionsValue;
        public TextMeshProUGUI DerpusEnergy;
        public TextMeshProUGUI DerpusMorale;

        public List<GameObject> CompanionStatuses;
        public Transform CompanionStatusParent;
        public GameObject CompanionStatusPrefab;

        public GameObject ScrollPartyLeftButton;
        public GameObject ScrollPartyRightButton;

        private void Start()
        {
             Populate();
             SubscribeToEvents();
        }

        public void Populate()
        {
            var party = TravelManager.Instance.Party;

            GoldValue.text = party.Gold.ToString();
            FoodValue.text = party.Food.ToString();
            HealthPotionsValue.text = party.HealthPotions.ToString();
            DerpusEnergy.text = $"{party.Derpus.Stats.CurrentEnergy}/{party.Derpus.Stats.MaxEnergy}";
            DerpusMorale.text = $"{party.Derpus.Stats.CurrentMorale}/{party.Derpus.Stats.MaxMorale}";

            PopulateCompanionStatuses(party);
        }

        private void PopulateCompanionStatuses(Party party)
        {
            if (CompanionStatusParent != null && CompanionStatusParent.gameObject != null)
            {
                GlobalHelper.DestroyAllChildren(CompanionStatusParent.gameObject);
            }

            CompanionStatuses = new List<GameObject>();

            foreach (var companion in party.GetCompanions())
            {
                var companionStatus = Instantiate(CompanionStatusPrefab, Vector3.zero, Quaternion.identity);

                CompanionStatuses.Add(companionStatus);

                companionStatus.transform.SetParent(CompanionStatusParent);

                var script = companionStatus.GetComponentInChildren<CompanionStatus>();

                if (script != null)
                {
                    script.Populate(companion);
                }
            }

            _currentStartingIndex = 0;

            if (CompanionStatuses.Count > MaxStatusesDisplayed)
            {
                //display first 4 companion statuses
                for (var i = 0; i < MaxStatusesDisplayed; i++)
                {
                    CompanionStatuses[i].GetComponentInChildren<CompanionStatus>().Show();
                }

                //hide the rest
                for (var i = MaxStatusesDisplayed; i < CompanionStatuses.Count; i++)
                {
                    CompanionStatuses[i].GetComponentInChildren<CompanionStatus>().Hide();
                }

                if (ScrollPartyRightButton == null || ScrollPartyLeftButton == null)
                {
                    return;
                }

                ScrollPartyLeftButton.SetActive(false);
                ScrollPartyRightButton.SetActive(true);
            }
        }

        //todo maybe polish to keep from resetting party status list to first each time
        public void Refresh()
        {
            var party = TravelManager.Instance.Party;

            GoldValue.text = party.Gold.ToString();
            FoodValue.text = party.Food.ToString();
            HealthPotionsValue.text = party.HealthPotions.ToString();
            DerpusEnergy.text = $"{party.Derpus.Stats.CurrentEnergy}/{party.Derpus.Stats.MaxEnergy}";
            DerpusMorale.text = $"{party.Derpus.Stats.CurrentMorale}/{party.Derpus.Stats.MaxMorale}";
        }

        public void ScrollPartyListLeft()
        {
            if (_currentStartingIndex == 0)
            {
                ScrollPartyLeftButton.SetActive(false);
                return;
            }

            _currentStartingIndex--;

            var (minIndex, maxIndex) = (_currentStartingIndex, _currentStartingIndex + MaxStatusesDisplayed);
            
            var index = 0;
            foreach (var status in CompanionStatuses)
            {
                if (index >= minIndex && index < maxIndex)
                {
                    status.GetComponentInChildren<CompanionStatus>().Show();
                }
                else
                {
                    status.GetComponentInChildren<CompanionStatus>().Hide();
                }

                index++;
            }

            if (_currentStartingIndex == 0)
            {
                ScrollPartyLeftButton.SetActive(false);
            }

            ScrollPartyRightButton.SetActive(true);
        }

        public void ScrollPartyListRight()
        {
            if (_currentStartingIndex == CompanionStatuses.Count - MaxStatusesDisplayed)
            {
                ScrollPartyRightButton.SetActive(false);
                ScrollPartyLeftButton.SetActive(true);
                return;
            }

            _currentStartingIndex++;

            var (minIndex, maxIndex) = (_currentStartingIndex, _currentStartingIndex + MaxStatusesDisplayed);

            var index = 0;
            foreach (var status in CompanionStatuses)
            {
                if (index >= minIndex && index < maxIndex)
                {
                    status.GetComponentInChildren<CompanionStatus>().Show();
                }
                else
                {
                    status.GetComponentInChildren<CompanionStatus>().Hide();
                }

                index++;
            }

            if (_currentStartingIndex == CompanionStatuses.Count - MaxStatusesDisplayed)
            {
                ScrollPartyRightButton.SetActive(false);
            }

            ScrollPartyLeftButton.SetActive(true);
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
                Populate();
            }
        }
    }
}
