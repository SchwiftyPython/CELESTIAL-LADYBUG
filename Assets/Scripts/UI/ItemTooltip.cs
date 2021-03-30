using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText = null;
        [SerializeField] private TextMeshProUGUI _bodyText = null;

        public void Setup(Item item)
        {
            _titleText.text = item.GetDisplayName();
            _bodyText.text = item.GetDescription();
        }
    }
}
