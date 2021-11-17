using System;
using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class NewGameWindow : MonoBehaviour
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

        public void Show()
        {
            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(uiContainer);
        }

        public void Hide()
        {
            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(uiContainer);
        }

        public void StartGame()
        {
            if (string.IsNullOrEmpty(_saveFileName) ||
                _saveFileName.Equals("SaveFile.es3", StringComparison.OrdinalIgnoreCase))
            {
                _saveFileName = "SlotOne";
            }

            if (_savingSystem.SaveExists(_saveFileName))
            {
                var overwriteConfirm = FindObjectOfType<OverwriteSavedGameWindow>();

                overwriteConfirm.Show(_saveFileName);
            }
            else
            {
                GameManager.Instance.StartNewGame(_saveFileName);
            }
        }

        public void SetNewSaveFileName(string fileName)
        {
            _saveFileName = fileName;
        }
    }
}
