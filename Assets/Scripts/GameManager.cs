using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Audio;
using Assets.Scripts.Items;
using Assets.Scripts.Travel;
using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour, ISaveable
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
        public const string TravelSceneName = "Travel";
        public const int TravelSceneIndex = 1;
        public const string CombatSceneName = "Combat";
        public const int CombatSceneIndex = 2;

        private List<string> _subscribedEventNames;

        private List<GameObject> _activeWindows;

        private GameState _currentState;

        private MusicController _musicController;

        public string SaveFileName;

        public static Scene CurrentScene => SceneManager.GetActiveScene();

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

            //CurrentScene = SceneManager.GetActiveScene();

            _activeWindows = new List<GameObject>();

            SubscribeToEvents();
        }

        private void Start()
        {
            _musicController = FindObjectOfType<MusicController>();

            _musicController.PlayTitleMusic();

            var spriteStore = FindObjectOfType<SpriteStore>();
            spriteStore.Setup();

            var itemStore = FindObjectOfType<ItemStore>();
            itemStore.Setup();
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

        public bool InCombat()
        {
            return SceneManager.GetActiveScene().name.Equals(CombatSceneName);
        }

        public void StartNewGame(string saveFileName)
        {
            _musicController.EndTitleMusic();

            LoadTravelScene();

            var travelManager = FindObjectOfType<TravelManager>();

            travelManager.NewParty();

            travelManager.NewInventory();

            travelManager.StartNewDay();

            var savingSystem = FindObjectOfType<SavingSystem>();  //might have to wait for travel scene to load

            savingSystem.Save(saveFileName);
        }

        public void LoadTravelScene()
        {
            StartCoroutine(WaitForSceneLoad(TravelSceneIndex));
        }

        public void LoadCombatScene()
        {
            StartCoroutine(WaitForSceneLoad(CombatSceneIndex));
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {

        }

        public void WaitForSeconds(int numSeconds)
        {
            StartCoroutine(WaitForSecondsCoroutine(numSeconds));
        }

        private IEnumerator WaitForSceneLoad(int sceneNumber)
        {
            yield return SceneManager.LoadSceneAsync(sceneNumber);
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
            //     //eventMediator.SubscribeToEvent(eventName, this);
            // }
        }

        private void UnsubscribeFromEvents()
        {
            //todo
            //eventMediator.UnsubscribeFromAllEvents(this);
        }

        public struct GameManagerDto
        {
            public string CurrentSceneName;
        }

        public object CaptureState()
        {
            var dto = new GameManagerDto
            {
                CurrentSceneName = CurrentScene.name
            };

            return dto;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }

            var savedScene = ((GameManagerDto)state).CurrentSceneName;

            if (string.Equals(savedScene, TravelSceneName, StringComparison.OrdinalIgnoreCase))
            {
                if (CurrentScene.buildIndex == TravelSceneIndex)
                {
                    return;
                }

                LoadTravelScene();
            }
            else if (string.Equals(savedScene, CombatSceneName, StringComparison.OrdinalIgnoreCase))
            {
                if (CurrentScene.buildIndex == CombatSceneIndex)
                {
                    return;
                }

                LoadCombatScene();
            }
        }
    }
}
