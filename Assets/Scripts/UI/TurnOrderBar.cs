using System.Collections.Generic;
using Assets.Scripts.Combat;
using UnityEngine;

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
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(RefreshEvent, this);
        }

        //todo refactor
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

            //todo for each slot
            foreach (var slot in slotList)
            {
                GlobalHelper.DestroyAllChildren(slot.gameObject);
            }

            var combatManager = Object.FindObjectOfType<CombatManager>();
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

                var entityInstance = Instantiate(entity.CombatSpritePrefab, new Vector3(0, 0), Quaternion.identity);

                var spriteStore = FindObjectOfType<SpriteStore>();

                var colorSwapper = entityInstance.GetComponentsInChildren<ColorSwapper>();

                spriteStore.SetColorSwaps(colorSwapper, entity);

                entityInstance.gameObject.GetComponent<Animator>().enabled = false;

                entityInstance.transform.SetParent(slotList[count]);

                entityInstance.transform.localPosition = new Vector3(-0.075f, -0.3f);

                entityInstance.transform.localScale = new Vector3(0.15f, 0.8f);

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
