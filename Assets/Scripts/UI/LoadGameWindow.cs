using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LoadGameWindow : MonoBehaviour
    {
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private GameObject slotOne = null;
        [SerializeField] private GameObject slotTwo = null;
        [SerializeField] private GameObject slotThree = null;
        [SerializeField] private GameObject slotFour = null;

        private SavingSystem _savingSystem;


        private void Start()
        {
            _savingSystem = FindObjectOfType<SavingSystem>();
        }

    
    }
}
