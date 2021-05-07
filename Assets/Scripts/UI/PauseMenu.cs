using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class PauseMenu : MonoBehaviour, ISubscriber
    {
        private const string ShowPopupEvent = GlobalHelper.ShowPauseMenu;
        private const string HidePopupEvent = GlobalHelper.HidePauseMenu;

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            EventMediator eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(HidePopupEvent, this);
            eventMediator.UnsubscribeFromEvent(ShowPopupEvent, this);

            eventMediator.Broadcast(GlobalHelper.PauseTimer, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        public void Hide()
        {
            EventMediator eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.UnsubscribeFromEvent(HidePopupEvent, this);
            eventMediator.SubscribeToEvent(ShowPopupEvent, this);

            eventMediator.Broadcast(GlobalHelper.ResumeTimer, this);

            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(GlobalHelper.TitleScreenScene);
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
