using System;
using Assets.Scripts.Combat;
using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class PauseMenu : MonoBehaviour, ISubscriber
    {
        private const string ShowPopupEvent = GlobalHelper.ShowPauseMenu;
        private const string HidePopupEvent = GlobalHelper.HidePauseMenu;

        [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private GameObject boardHolder = null;
        [SerializeField] private GameObject entityHolder = null;

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
            if (GameManager.Instance.CurrentScene.name.Equals(GlobalHelper.CombatScene))
            {
                var combatInput = FindObjectOfType<CombatInputController>();

                if (combatInput.TileSelected() || combatInput.AbilitySelected())
                {
                    return;
                }

                uiContainer.SetActive(true);
                GameManager.Instance.AddActiveWindow(uiContainer);

                return;
            }

            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(uiContainer);

            EventMediator eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.PauseTimer, this);
        }

        public void Hide()
        {
            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(uiContainer);

            if (GameManager.Instance.CurrentScene.name.Equals(GlobalHelper.CombatScene))
            {
                return;
            }

            if (GameManager.Instance.AnyActiveWindows())
            {
                return;
            }

            EventMediator eventMediator = FindObjectOfType<EventMediator>();

            //todo need to keep paused when inventory is open

            eventMediator.Broadcast(GlobalHelper.ResumeTimer, this);
        }

        public void Save() //todo disable on enemy turn
        {
            var saveSystem = FindObjectOfType<SavingSystem>();

            saveSystem.Save(String.Empty); //todo gen file name 

            //ES3AutoSaveMgr.Current.Save();
        }

        public void Load()
        {
            var combatManager = FindObjectOfType<CombatManager>();

            combatManager.LoadFromSave();

            GlobalHelper.DestroyAllChildren(entityHolder);
            GlobalHelper.DestroyAllChildren(boardHolder);

            var saveSystem = FindObjectOfType<SavingSystem>();

            saveSystem.Load(String.Empty); //todo file select

            //ES3AutoSaveMgr.Current.Load();
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
