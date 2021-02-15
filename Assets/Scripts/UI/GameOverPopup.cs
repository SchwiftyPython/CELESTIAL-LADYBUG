using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class GameOverPopup : MonoBehaviour, ISubscriber
    {
        private const string ShowPopupEvent = GlobalHelper.GameOver;
        private const string HidePopupEvent = GlobalHelper.HidePopup;

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(ShowPopupEvent, this);
            Hide();
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(GlobalHelper.TitleScreenScene);
        }

        private void Show()
        {
            EventMediator.Instance.SubscribeToEvent(HidePopupEvent, this);

            gameObject.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        private void Hide()
        {
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
            gameObject.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private void OnDestroy()
        {
            EventMediator.Instance.UnsubscribeFromEvent(ShowPopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
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
