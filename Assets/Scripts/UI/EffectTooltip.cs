using Assets.Scripts.Effects;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EffectTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText = null;
        [SerializeField] private TextMeshProUGUI _descriptionText = null;
        [SerializeField] private GameObject _durationTextParent = null;
        [SerializeField] private TextMeshProUGUI _durationText = null;
        [SerializeField] private TextMeshProUGUI _sourceText = null; //todo this might be a nice to have

        public void Setup(Effect effect)
        {
            _titleText.text = effect.Name;
            _descriptionText.text = effect.GetDescription();

            if (effect.Duration < 0)
            {
                _durationTextParent.SetActive(false);
            }
            else
            {
                _durationTextParent.SetActive(true);
                _durationText.text = effect.Duration.ToString();
            }

            if (effect.IsLocationDependent()) 
            {
                _sourceText.text = "From current tile";
            }
            else
            {
                _sourceText.text = "";
            }
        }
    }
}
