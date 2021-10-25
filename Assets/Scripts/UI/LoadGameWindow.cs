using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LoadGameWindow : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private GameObject slotOne = null;
        [SerializeField] private GameObject slotTwo = null;
        [SerializeField] private GameObject slotThree = null;
        [SerializeField] private GameObject slotFour = null;

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

        public void LoadSelectedGame()
        {
            _savingSystem.Load(_saveFileName);
        }

        public void SetNewSaveFileName(string fileName)
        {
            _saveFileName = fileName;
        }
    }
}
