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
        [SerializeField] private TextMeshProUGUI _itemGroup = null;
        [SerializeField] private TextMeshProUGUI _value = null;
        [SerializeField] private TextMeshProUGUI _meleeDamage = null;
        [SerializeField] private TextMeshProUGUI _rangedDamage = null;
        [SerializeField] private TextMeshProUGUI _toughness = null;
        [SerializeField] private TextMeshProUGUI _dodgeMod = null;

        public void Setup(Item item)
        {
            _titleText.text = item.GetDisplayName();
            _bodyText.text = item.GetDescription();

            var itemGroup = item.GetItemGroupForDisplay();

            if (item.IsWeapon())
            {
                itemGroup += $", {(item.IsTwoHanded() ? "Two-Handed" : "One-Handed")}";
            }

            _itemGroup.text = $"{itemGroup}"; 
            _value.text = $"Value {item.GetValue()} gold";

            var meleeAttack = item.GetMeleeAttack();

            if (meleeAttack == null || meleeAttack.MinDamage == 0 && meleeAttack.MaxDamage == 0)
            {
                _meleeDamage.gameObject.SetActive(false);
            }
            else
            {
                _meleeDamage.text = $"Melee Damage {meleeAttack.MinDamage} - {meleeAttack.MaxDamage}";
                _meleeDamage.gameObject.SetActive(true);
            }

            var rangedAttack = item.GetRangedAttack();

            if (rangedAttack == null || rangedAttack.MinDamage == 0 && rangedAttack.MaxDamage == 0)
            {
                _rangedDamage.gameObject.SetActive(false);
            }
            else
            {
                _rangedDamage.text = $"Ranged Damage {rangedAttack.MinDamage} - {rangedAttack.MaxDamage}";
                _rangedDamage.gameObject.SetActive(true);
            }

            var defense = item.GetDefense();

            if (defense == null || defense.Toughness == 0 && defense.DodgeMod == 0)
            {
                _toughness.gameObject.SetActive(false);
                _dodgeMod.gameObject.SetActive(false);
            }
            else
            {
                _toughness.text = $"Toughness {defense.Toughness}";
                _toughness.gameObject.SetActive(true);

                _dodgeMod.text = $"Dodge Mod {defense.DodgeMod}";
                _dodgeMod.gameObject.SetActive(true);
            }
        }
    }
}
