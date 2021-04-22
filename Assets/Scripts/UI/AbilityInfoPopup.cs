using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    //todo refactor with popup
    public class AbilityInfoPopup : MonoBehaviour, ISubscriber
    {
        private const string HoverPopupEvent = GlobalHelper.AbilityHovered;
        private const string HidePopupEvent = GlobalHelper.HidePopup;

        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _abilityDescription;

        [SerializeField]
        private TextMeshProUGUI _apCost;

        [SerializeField]
        private TextMeshProUGUI _damageDescription; 

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(HoverPopupEvent, this);
            Hide();
        }

        private void Show(Ability ability, int baseDamage)
        {
            _name.text = GlobalHelper.Capitalize(ability.Name);
            _abilityDescription.text = "Description not implemented yet"; //todo
            _apCost.text = ability.ApCost.ToString();

            //todo probably need bool for if ability damage is based on weapon
            if (ability.HostileTargetsOnly)
            {
                int damageMin;
                int damageMax;
                if (ability.IsRanged())
                {
                    (damageMin, damageMax) = CombatManager.Instance.ActiveEntity.GetEquippedWeapon().GetRangedDamageRange();
                }
                else
                {
                    (damageMin, damageMax) = CombatManager.Instance.ActiveEntity.GetEquippedWeapon().GetMeleeDamageRange();
                }

                _damageDescription.text = $"Deals {damageMin + baseDamage} - {damageMax + baseDamage} damage";
            }

            var position = Input.mousePosition;
            gameObject.transform.position = new Vector2(position.x + 180f, position.y + 160f);

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
            EventMediator.Instance.UnsubscribeFromEvent(HoverPopupEvent, this);
            EventMediator.Instance.UnsubscribeFromEvent(HidePopupEvent, this);
            GameManager.Instance.RemoveActiveWindow(gameObject);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(HoverPopupEvent))
            {
                //todo bug here probably checking parameter

                if (GameManager.Instance.AnyActiveWindows())
                {
                    return;
                }

                // if (!(broadcaster is Entity abilityOwner))
                // {
                //     return;
                // }

                if (!(parameter is Ability ability))
                {
                    return;
                }

                Show(ability, ability.AbilityOwner.Stats.Attack);
            }
            else if (eventName.Equals(HidePopupEvent))
            {
                Hide();
            }
        }
    }
}
