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
        private const int TooltipWidthNormal = 155;
        private const int TooltipWidthHitChance = 215;
        private const int GreenChanceMin = 75;
        private const int YellowChanceMin = 45;

        [SerializeField]
        private TextMeshProUGUI entityName;

        [SerializeField]
        private TextMeshProUGUI health;

        [SerializeField]
        private TextMeshProUGUI hitChanceValue;
        [SerializeField]
        private TextMeshProUGUI hitChanceLabel;
        [SerializeField]
        private GameObject hitChanceParent;
        [SerializeField]
        private GameObject negativesParent;
        [SerializeField]
        private GameObject positivesParent;
        [SerializeField]
        private GameObject positivesPrefab;
        [SerializeField]
        private GameObject negativesPrefab;

        [SerializeField]
        private TextMeshProUGUI nextTurnText;

        public void Setup(Entity targetEntity)
        {
            //todo should show equipped armor toughness

            entityName.text = targetEntity.Name;
            health.text = $"{targetEntity.Stats.CurrentHealth}/{targetEntity.Stats.MaxHealth}";

            var inputController = FindObjectOfType<CombatInputController>();

            if (inputController.AbilitySelected())
            {
                if (!inputController.TargetValid(targetEntity))
                {
                    hitChanceValue.text = "0%";
                    hitChanceValue.color =  Color.red; //FindObjectOfType<Palette>().BrightRed; 
                    hitChanceLabel.text = "Invalid Target";
                    hitChanceParent.SetActive(true);
                    positivesParent.SetActive(false);
                    negativesParent.SetActive(false);

                    GetComponent<RectTransform>()
                        .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TooltipWidthNormal);
                }
                else if (!inputController.TargetInRange(targetEntity))
                {
                    hitChanceValue.text = "0%";
                    hitChanceValue.color = Color.red; //FindObjectOfType<Palette>().BrightRed; 
                    hitChanceLabel.text = "Out of Range";
                    hitChanceParent.SetActive(true);
                    positivesParent.SetActive(false);
                    negativesParent.SetActive(false);

                    GetComponent<RectTransform>()
                        .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TooltipWidthNormal);
                }
                else
                {
                    var hitChanceTuple = inputController.GetHitChance(targetEntity);

                    hitChanceValue.text = $"{hitChanceTuple.hitChance}%";
                    hitChanceLabel.text = "chance to hit";

                    if (hitChanceTuple.hitChance >= GreenChanceMin)
                    {
                        hitChanceValue.color = Color.green;
                    }
                    else if (hitChanceTuple.hitChance >= YellowChanceMin)
                    {
                        hitChanceValue.color = Color.yellow;
                    }
                    else
                    {
                        hitChanceValue.color = Color.red;
                    }

                    hitChanceParent.SetActive(true);

                    var positives = hitChanceTuple.positives;

                    if (positives != null && positives.Any())
                    {
                        GlobalHelper.DestroyAllChildren(positivesParent);

                        foreach (var positive in positives)
                        {
                            var posInstance = Instantiate(positivesPrefab, positivesParent.transform);

                            posInstance.GetComponentInChildren<TextMeshProUGUI>().text = positive;
                        }

                        positivesParent.SetActive(true);
                    }
                    else
                    {
                        positivesParent.SetActive(false);
                    }

                    var negatives = hitChanceTuple.negatives;

                    if (negatives != null && negatives.Any())
                    {
                        GlobalHelper.DestroyAllChildren(negativesParent);

                        foreach (var negative in negatives)
                        {
                            var negInstance = Instantiate(negativesPrefab, negativesParent.transform);

                            negInstance.GetComponentInChildren<TextMeshProUGUI>().text = negative;
                        }

                        negativesParent.SetActive(true);
                    }
                    else
                    {
                        negativesParent.SetActive(false);
                    }

                    if (positivesParent.activeSelf || negativesParent.activeSelf)
                    {
                        GetComponent<RectTransform>()
                            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TooltipWidthHitChance);
                    }
                    else
                    {
                        GetComponent<RectTransform>()
                            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TooltipWidthNormal);
                    }
                }
            }
            else
            {
                hitChanceParent.SetActive(false);
                positivesParent.SetActive(false);
                negativesParent.SetActive(false);

                GetComponent<RectTransform>()
                    .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TooltipWidthNormal);
            }

            var combatManager = FindObjectOfType<CombatManager>();
            var nextTurn = combatManager.TurnOrder.ToList().IndexOf(targetEntity);

            nextTurnText.text = $"Acts in {nextTurn} turns";
        }
    }
}
