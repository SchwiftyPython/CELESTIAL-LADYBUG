using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GenericToolTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltipText;

        public void Setup(string tooltip)
        {
            tooltipText.text = tooltip;
        }
    }
}
