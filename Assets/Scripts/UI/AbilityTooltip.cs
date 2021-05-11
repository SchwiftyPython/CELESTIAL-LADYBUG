using Assets.Scripts.Abilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AbilityTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText = null;
        [SerializeField] private TextMeshProUGUI _bodyText = null;
        [SerializeField] private GameObject _apDamageParent = null;
        [SerializeField] private TextMeshProUGUI _apCostText = null;
        [SerializeField] private TextMeshProUGUI _damageText = null;

        public void Setup(Ability ability)
        {
            _titleText.text = ability.Name;
            _bodyText.text = ability.Description;

            if (ability.IsPassive)
            {
                _apDamageParent.SetActive(false);

                // _apCostText.gameObject.SetActive(false);
                // _damageText.gameObject.SetActive(false);
            }
            else
            {
                _apCostText.text = $"{ability.ApCost} AP";

                var (damageMin, damageMax) = ability.GetAbilityDamageRange();

                _damageText.text = $"Deals {damageMin} - {damageMax} damage";

                _apDamageParent.SetActive(true);

                // _apCostText.gameObject.SetActive(true);
                // _damageText.gameObject.SetActive(true);
            }
        }
    }
}
