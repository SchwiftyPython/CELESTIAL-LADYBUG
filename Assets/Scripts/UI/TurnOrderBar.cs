using System.Collections.Generic;
using Assets.Scripts.Combat;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TurnOrderBar : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.RefreshCombatUi;
        private const int MaxSprites = 7;

        //todo for prototype only - get from the UiSprite in Entity class later
        public GameObject PlayerSprite;
        public GameObject EnemySprite;

        public Transform SlotOne;
        public Transform SlotTwo;
        public Transform SlotThree;
        public Transform SlotFour;
        public Transform SlotFive;
        public Transform SlotSix;
        public Transform SlotSeven;

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(RefreshEvent, this);
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

            var turnOrder = CombatManager.Instance.TurnOrder;

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

                GameObject spriteParentPrefab;
                if (entity.IsPlayer())
                {
                    spriteParentPrefab = PlayerSprite;
                }
                else
                {
                    spriteParentPrefab = EnemySprite;
                }

                var instance = Instantiate(spriteParentPrefab, new Vector3(0, 0), Quaternion.identity);

                instance.gameObject.GetComponent<Animator>().enabled = false;

                instance.transform.SetParent(slotList[count]);

                instance.transform.localPosition = new Vector3(-0.075f, -0.3f);

                instance.transform.localScale = new Vector3(0.15f, 0.8f);

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
