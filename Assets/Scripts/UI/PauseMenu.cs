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
            //EventMediator.Instance.SubscribeToEvent(ShowPopupEvent, this);
            Hide();
        }

        public void Show()
        {
            EventMediator.Instance.SubscribeToEvent(HidePopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(ShowPopupEvent, this);

            EventMediator.Instance.Broadcast(GlobalHelper.PauseTimer, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        public void Hide()
        {
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
            EventMediator.Instance.SubscribeToEvent(ShowPopupEvent, this);

            EventMediator.Instance.Broadcast(GlobalHelper.ResumeTimer, this);

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
