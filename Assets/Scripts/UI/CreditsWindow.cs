using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CreditsWindow : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private Transform creditsTextTransform = null;
        [SerializeField] private float creditsEndPos;
        [SerializeField] private float creditsStartPos;
        [SerializeField] private float duration;

        private void Start()
        {
            if (uiContainer.activeSelf)
            {
                Hide();
            }
        }

        public void Show()
        {
            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(uiContainer);

            StartCoroutine(ScrollCredits());
        }

        public void Hide()
        {
            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(uiContainer);

            creditsTextTransform.DOLocalMoveY(creditsStartPos, 0.1f);
        }

        private IEnumerator ScrollCredits()
        {
            yield return new WaitForSecondsRealtime(0.35f);

            creditsTextTransform.DOLocalMoveY(creditsEndPos, duration, true);
        }
    }
}
