using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PauseMenu : MonoBehaviour, ISubscriber
    {
        private const string ShowPopupEvent = GlobalHelper.ShowPauseMenu;
        private const string HidePopupEvent = GlobalHelper.HidePauseMenu;
        private const string PlayerTurnEvent = GlobalHelper.PlayerTurn;
        private const string AiTurnEvent = GlobalHelper.AiTurn;

        [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private Button QuitButton = null;
        [SerializeField] private Button RetreatButton = null;
        [SerializeField] private GameObject entityHolder = null;
        [SerializeField] private GameObject boardHolder = null;

        private void Start()
        {
            if (uiContainer.activeSelf)
            {
                Hide();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (uiContainer.activeSelf)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
        }


        public void Show()
        {
            if (GameManager.CurrentScene.name.Equals(GlobalHelper.CombatScene))
            {
                var combatInput = FindObjectOfType<CombatInputController>();

                if (combatInput.TileSelected() || combatInput.AbilitySelected())
                {
                    return;
                }

                uiContainer.SetActive(true);
                GameManager.Instance.AddActiveWindow(uiContainer);

                var combatManager = FindObjectOfType<CombatManager>();

                QuitButton.interactable = combatManager.IsPlayerTurn();
                RetreatButton.interactable = combatManager.IsPlayerTurn();

                return;
            }

            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(uiContainer);

            EventMediator eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.PauseTimer, this);
        }

        public void Hide()
        {
            var loadWindow = GameObject.Find("LoadGameMask");
            var settingsWindow = GameObject.Find("SettingsWindowMask");

            EventMediator eventMediator = FindObjectOfType<EventMediator>();

            if (GameManager.Instance.WindowActive(settingsWindow) || GameManager.Instance.WindowActive(loadWindow))
            {
                return;
            }           

            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(uiContainer);

            if (GameManager.Instance.AnyActiveWindows())
            {
                return;
            }

            eventMediator.Broadcast(GlobalHelper.ResumeTimer, this);
        }

        public void SaveAndQuit()
        {
            var saveSystem = FindObjectOfType<SavingSystem>();

            saveSystem.AutoSave();

            LoadMainMenuScene();
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(GlobalHelper.TitleScreenScene);
        }

        public void Retreat()
        {
            Hide();

            var retreatPopup = FindObjectOfType<RetreatFromCombatPopup>();

            retreatPopup.Show();
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
