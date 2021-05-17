using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EntityTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private TextMeshProUGUI _health;

        [SerializeField]
        private TextMeshProUGUI _hitChance;
        [SerializeField]
        private GameObject _hitChanceParent;

        [SerializeField]
        private TextMeshProUGUI _nextTurn;

        public void Setup(Entity targetEntity)
        {
            //todo should show equipped armor toughness

            _name.text = targetEntity.Name;
            _health.text = $"{targetEntity.Stats.CurrentHealth}/{targetEntity.Stats.MaxHealth}";

            var inputController = FindObjectOfType<CombatInputController>();

            if (inputController.AbilitySelected() && inputController.TargetValid(targetEntity) &&
                inputController.TargetInRange(targetEntity))
            {
                //todo list hit chance modifiers
                _hitChance.text = $"{inputController.GetHitChance(targetEntity)}% Chance to hit";
                _hitChanceParent.SetActive(true);
            }
            else
            {
                _hitChanceParent.SetActive(false);
            }

            var combatManager = Object.FindObjectOfType<CombatManager>();
            var nextTurn = combatManager.TurnOrder.ToList().IndexOf(targetEntity);

            _nextTurn.text = $"Acts in {nextTurn} turns";
        }
    }
}
