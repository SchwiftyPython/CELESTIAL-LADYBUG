using System.Linq;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.UI
{
    public class EntityTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI entityName;

        [SerializeField]
        private TextMeshProUGUI health;

        [SerializeField]
        private TextMeshProUGUI hitChance;
        [SerializeField]
        private GameObject hitChanceParent;
        [SerializeField]
        private GameObject negativesParent;
        [SerializeField]
        private GameObject positivesParent;

        [SerializeField]
        private TextMeshProUGUI nextTurnText;

        public void Setup(Entity targetEntity)
        {
            //todo should show equipped armor toughness

            entityName.text = targetEntity.Name;
            health.text = $"{targetEntity.Stats.CurrentHealth}/{targetEntity.Stats.MaxHealth}";

            var inputController = FindObjectOfType<CombatInputController>();

            if (inputController.AbilitySelected() && inputController.TargetValid(targetEntity) &&
                inputController.TargetInRange(targetEntity))
            {
                //todo list hit chance modifiers
                //todo could use a method for getting hit chance positives and negatives
                hitChance.text = $"{inputController.GetHitChance(targetEntity)}% Chance to hit";
                hitChanceParent.SetActive(true);
            }
            else
            {
                hitChanceParent.SetActive(false);
            }

            var combatManager = Object.FindObjectOfType<CombatManager>();
            var nextTurn = combatManager.TurnOrder.ToList().IndexOf(targetEntity);

            nextTurnText.text = $"Acts in {nextTurn} turns";
        }
    }
}
