using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Travel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private enum GameState
        {
            Title,
            NewGame,
            LoadGame,
            Travel,
            Combat,
            Encounter,
            GameOver,
            Save
        }

        private const string TitleScreenSceneName = "TitleScreen";
        private const string TravelSceneName = "Travel";
        private const string CombatSceneName = "Combat";

        private List<string> _subscribedEventNames;

        private List<GameObject> _activeWindows;

        private GameState _currentState;

        public Scene CurrentScene { get; private set; }

        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            _currentState = GameState.Title;

            CurrentScene = SceneManager.GetActiveScene();

            _activeWindows = new List<GameObject>();

            SubscribeToEvents();
        }

        private void Update()
        {
            switch (_currentState)
            {
                case GameState.Title:
                    break;
                case GameState.NewGame:
                    break;
                case GameState.LoadGame:
                    break;
                case GameState.Travel:
                    break;
                case GameState.Combat:
                    break;
                case GameState.Encounter:
                    break;
                case GameState.GameOver:
                    break;
                case GameState.Save:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddActiveWindow(GameObject window)
        {
            if (_activeWindows.Contains(window))
            {
                return;
            }
            _activeWindows.Add(window);
        }

        public void RemoveActiveWindow(GameObject window)
        {
            if (!_activeWindows.Contains(window))
            {
                return;
            }
            _activeWindows.Remove(window);
        }

        public bool AnyActiveWindows()
        {
            return _activeWindows.Any();
        }

        public bool WindowActive(GameObject window)
        {
            foreach (var activeWindow in _activeWindows)
            {
                if (ReferenceEquals(activeWindow, window))
                {
                    return true;
                }
            }

            return false;
        }

        public void StartNewGame()
        {
            //todo BUG this in fact does not start a new game lmao

            LoadTravelScene();

            TravelManager.Instance.NewParty();

            TravelManager.Instance.NewInventory();

            TravelManager.Instance.StartNewDay();
        }

        public void LoadTravelScene()
        {
            SceneManager.LoadScene(TravelSceneName);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {

        }

        public void WaitForSeconds(int numSeconds)
        {
            StartCoroutine(WaitForSecondsCoroutine(numSeconds));
        }

        private IEnumerator WaitForSecondsCoroutine(int numSeconds)
        {
            yield return new WaitForSeconds(numSeconds);
        }

        private void SubscribeToEvents()
        {
            // foreach (var eventName in _subscribedEventNames)
            // {
            //     //todo
            //     //EventMediator.Instance.SubscribeToEvent(eventName, this);
            // }
        }

        private void UnsubscribeFromEvents()
        {
            //todo
            //EventMediator.Instance.UnsubscribeFromAllEvents(this);
        }
    }
}
