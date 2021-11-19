using System.Collections;
using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class DemoPopup : MonoBehaviour, ISubscriber
    {
        private const string ShowPopupEvent = GlobalHelper.YouWon;
        private const string HidePopupEvent = GlobalHelper.HidePopup;

        [SerializeField] private GameObject uiContainer = null;

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            if (GameManager.Instance.DemoMode)
            {
                eventMediator.SubscribeToEvent(ShowPopupEvent, this);
            }

            Hide();
        }

        public void LoadMainMenuScene()
        {
            var saveSystem = FindObjectOfType<SavingSystem>();

            saveSystem.DeleteCurrentSave();

            SceneManager.LoadScene(GlobalHelper.TitleScreenScene);
        }

        private IEnumerator DelayedStart()
        {
            yield return Delay();
        }

        private void Show()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(HidePopupEvent, this);

            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(gameObject);
        }

        private void Hide()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.UnsubscribeFromEvent(HidePopupEvent, this);
            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private void OnDestroy()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            if (eventMediator == null)
            {
                return;
            }

            eventMediator.UnsubscribeFromEvent(ShowPopupEvent, this);
            eventMediator.UnsubscribeFromEvent(HidePopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.0f);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(ShowPopupEvent))
            {
                StartCoroutine(DelayedStart());

                Show();
            }
            else if (eventName.Equals(HidePopupEvent))
            {
                Hide();
            }
        }
    }
}
