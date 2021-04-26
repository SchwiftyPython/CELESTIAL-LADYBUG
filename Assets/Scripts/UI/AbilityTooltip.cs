using Assets.Scripts.Abilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AbilityTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText = null;
        [SerializeField] private TextMeshProUGUI _bodyText = null;

        public void Setup(Ability ability)
        {
            _titleText.text = ability.Name;
            _bodyText.text = ability.Description;
        }
    }
}
