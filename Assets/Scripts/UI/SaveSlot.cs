using Assets.Scripts.Utilities.Save_Load;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class SaveSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI dayText = null;
        [SerializeField] private TextMeshProUGUI emptyText = null;
        [SerializeField] private TextMeshProUGUI dateTimeText = null;

        [SerializeField] private string saveFileName = string.Empty;

        [SerializeField] private LoadGameWindow loadGameWindow = null;
        [SerializeField] private NewGameWindow newGameWindow = null;

        private SavingSystem _savingSystem;

        public struct SaveGameInfo
        {
            public string DayInfo;
            public string DateTimeInfo;
        }

        private void Start()
        {
            _savingSystem = FindObjectOfType<SavingSystem>();

            PopulateSaveSlot();
        }

        private void PopulateSaveSlot()
        {
            var saveGameInfo = GetSaveGameInfo();

            if (saveGameInfo == null)
            {
                ShowEmptySlot();
                return;
            }

            emptyText.gameObject.SetActive(false);

            dayText.text = saveGameInfo.Value.DayInfo;
            dateTimeText.text = saveGameInfo.Value.DateTimeInfo;

            dayText.gameObject.SetActive(true);
            dateTimeText.gameObject.SetActive(true);
        }

        private SaveGameInfo? GetSaveGameInfo()
        {
            return _savingSystem.GetSaveGameInfo(saveFileName);
        }

        private void ShowEmptySlot()
        {
            dayText.gameObject.SetActive(false);
            dateTimeText.gameObject.SetActive(false);

            emptyText.gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (loadGameWindow != null)
            {
                loadGameWindow.SetNewSaveFileName(saveFileName);
            }
            else if (newGameWindow != null)
            {
                newGameWindow.SetNewSaveFileName(saveFileName);
            }
        }
    }
}
