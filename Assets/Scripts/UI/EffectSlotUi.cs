using Assets.Scripts.Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class EffectSlotUi : MonoBehaviour, IEffectHolder
    {
        private Effect _effect;

        public GameObject IconImageParent;

        public void SetEffect(Effect effect)
        {
            _effect = effect;

            var iconImage = IconImageParent.GetComponent<Image>();

            iconImage.sprite = effect.Icon;
        }

        public Effect GetEffect()
        {
            return _effect;
        }
    }
}
