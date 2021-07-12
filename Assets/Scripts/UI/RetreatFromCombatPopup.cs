using UnityEngine;

namespace Assets.Scripts.UI
{
    public class RetreatFromCombatPopup : MonoBehaviour
    {
        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            Hide();
        }

        public void Retreat()
        {
            Hide();
            //todo tell Combat Manager to do retreat stuff
        }
    }
}
