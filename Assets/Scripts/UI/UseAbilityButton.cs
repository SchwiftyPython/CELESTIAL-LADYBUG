using Assets.Scripts.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UseAbilityButton : MonoBehaviour
    {
        private Image _buttonIcon;
        private Button _button;

        public Ability Ability { get; private set; }

        private void Start()
        {
            _buttonIcon = gameObject.GetComponent<Image>();
            _button = gameObject.GetComponent<Button>();
        }

        public void AssignAbility(Ability ability, Sprite icon)
        {
            if (ability == null)
            {
                return;
            }

            if (_button == null)
            {
                _button = gameObject.GetComponent<Button>();
            }

            if (_buttonIcon == null)
            {
                _buttonIcon = gameObject.GetComponent<Image>();
            }

            Ability = ability;

            EnableButton();

            SetIcon(icon);
        }

        public void EnableButton()
        {
            _button.interactable = true;
        }

        public void DisableButton()
        {
            _button.interactable = false; 
        }

        public void OnClick()
        {
            if (!_button.interactable)
            {
                return;
            }

            //todo get all targets in range
            //todo select first target
        }

        private void SetIcon(Sprite icon)
        {
            _buttonIcon.sprite = icon;
        }
    }
}
