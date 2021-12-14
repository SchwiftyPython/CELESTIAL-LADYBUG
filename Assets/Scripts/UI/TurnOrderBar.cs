using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities.Companions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TurnOrderBar : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.RefreshCombatUi;
        private const int MaxSprites = 7;

        public Transform SlotOne;
        public Transform SlotTwo;
        public Transform SlotThree;
        public Transform SlotFour;
        public Transform SlotFive;
        public Transform SlotSix;
        public Transform SlotSeven;

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(RefreshEvent, this);
        }

        private void Populate()
        {
            var slotList = new List<Transform>
            {
                SlotOne,
                SlotTwo,
                SlotThree,
                SlotFour,
                SlotFive,
                SlotSix,
                SlotSeven
            };

            var combatManager = FindObjectOfType<CombatManager>();
            var turnOrder = combatManager.TurnOrder;

            if (turnOrder == null || turnOrder.Count < 1)
            {
                return;
            }

            int count = 0;
            foreach (var entity in turnOrder)
            {
                if (count >= MaxSprites)
                {
                    break;
                }

                if (entity.IsDead())
                {
                    continue;
                }

                var spriteStore = FindObjectOfType<SpriteStore>();

                Sprite tos;

                if (entity is Crossbowman || entity is Spearman || entity is ManAtArms)
                {
                    tos = spriteStore.GetTurnOrderSprite(entity);
                }
                else
                {
                    var spriteRenderer = entity.CombatSpriteInstance.GetComponent<SpriteRenderer>();

                    if (spriteRenderer == null)
                    {
                        spriteRenderer = entity.CombatSpriteInstance.GetComponentInChildren<SpriteRenderer>();
                    }

                    tos = spriteRenderer.sprite;
                }

                slotList[count].GetComponent<Image>().sprite = tos;

                if (!entity.IsPlayer())
                {
                    slotList[count].GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    slotList[count].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }

                count++;
            }
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(RefreshEvent))
            {
                Populate();
            }
        }
    }
}
