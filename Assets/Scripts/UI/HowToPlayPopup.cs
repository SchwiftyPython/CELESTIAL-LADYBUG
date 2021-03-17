using UnityEngine;

namespace Assets.Scripts.UI
{
    public class HowToPlayPopup : MonoBehaviour
    {
        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
