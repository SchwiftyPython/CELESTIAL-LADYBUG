using System.Collections.Generic;
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
            Quit
        }

        private const string TitleScreenSceneName = "TitleScreen";
        private const string TravelSceneName = "Travel";
        private const string CombatSceneName = "Combat";

        private Scene _currentScene;

        private List<string> _subscribedEventNames;

        private List<GameObject> _activeWindows;

        private GameState _currentState;

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
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
