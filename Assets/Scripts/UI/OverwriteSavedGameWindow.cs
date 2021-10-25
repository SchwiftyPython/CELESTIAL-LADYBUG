using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class OverwriteSavedGameWindow : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer = null;

        private SavingSystem _savingSystem;

        private string _saveFileName;

        private void Start()
        {
            if (uiContainer.activeSelf)
            {
                Hide();
            }

            _savingSystem = FindObjectOfType<SavingSystem>();
        }

        public void Show(string fileName)
        {
            _saveFileName = fileName;

            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(uiContainer);
        }

        public void Hide()
        {
            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(uiContainer);
        }

        public void Confirm()
        {
            Hide();

            _savingSystem.Delete(_saveFileName);

            var newGameWindow = FindObjectOfType<NewGameWindow>();

            newGameWindow.StartGame();
        }
    }
}
