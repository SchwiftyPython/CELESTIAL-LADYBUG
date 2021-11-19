using Assets.Scripts.Utilities.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class NewGamePopup : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private TextMeshProUGUI resultDescription;

        private TextWriter _textWriter;

        [FMODUnity.EventRef] public string popupSound;

        private void Awake()
        {
            if (uiContainer.activeSelf)
            {
                Hide();
            }

            _textWriter = GetComponent<TextWriter>();

            uiContainer.GetComponent<Button_UI>().ClickFunc = _textWriter.DisplayMessageInstantly;
        }

        public void Show()
        {
            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(uiContainer);

            if (_textWriter == null)
            {
                _textWriter = GetComponent<TextWriter>();
            }

            _textWriter.AddWriter(resultDescription, resultDescription.text, GlobalHelper.DefaultTextSpeed, true);

            var sound = FMODUnity.RuntimeManager.CreateInstance(popupSound);
            sound.start();

            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.PauseTimer, this);
        }

        public void Hide()
        {
            _textWriter.DisplayMessageInstantly();

            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(uiContainer);

            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.ResumeTimer, this);
        }
    }
}
