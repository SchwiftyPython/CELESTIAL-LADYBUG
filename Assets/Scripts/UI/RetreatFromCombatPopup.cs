using Assets.Scripts.Combat;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class RetreatFromCombatPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popup;

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            popup.SetActive(true);
        }

        private void Hide()
        {
            popup.SetActive(false);
        }

        public void Cancel()
        {
            Hide();
        }

        public void Retreat()
        {
            Hide();

            var combatManager = FindObjectOfType<CombatManager>();

            combatManager.Retreat();
        }
    }
}
