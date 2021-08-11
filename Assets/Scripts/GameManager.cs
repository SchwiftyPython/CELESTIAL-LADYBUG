﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Audio;
using Assets.Scripts.Items;
using Assets.Scripts.Travel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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

        private MusicController _musicController;

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

        private void Start()
        {
            _musicController = FindObjectOfType<MusicController>();

            _musicController.PlayTitleMusic();
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

        public void StartNewGame()
        {
            _musicController.EndTitleMusic();

            //todo BUG this in fact does not start a new game lmao

            var spriteStore = Object.FindObjectOfType<SpriteStore>();
            spriteStore.Setup();

            var itemStore = Object.FindObjectOfType<ItemStore>();
            itemStore.Setup();

            LoadTravelScene();

            var travelManager = Object.FindObjectOfType<TravelManager>();

            travelManager.NewParty();

            travelManager.NewInventory();

            travelManager.StartNewDay();
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
            //     //eventMediator.SubscribeToEvent(eventName, this);
            // }
        }

        private void UnsubscribeFromEvents()
        {
            //todo
            //eventMediator.UnsubscribeFromAllEvents(this);
        }
    }
}
