using System.Collections.Generic;
using Assets.Scripts.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TurnOrderBar : MonoBehaviour, ISubscriber
    {
        private const string RefreshEvent = GlobalHelper.RefreshCombatUi;
        private const int MaxSprites = 7;

        private Queue<GameObject> _sprites;

        //todo for prototype only - get from the UiSprite in Entity class later
        public Sprite PlayerSprite;
        public Sprite EnemySprite;

        public GameObject TurnOrderSpritePrefab;

        public Transform TurnOrderParent;

        private void Start()
        {
            EventMediator.Instance.SubscribeToEvent(RefreshEvent, this);
        }

        //todo refactor
        private void Populate()
        {
            GlobalHelper.DestroyAllChildren(TurnOrderParent.gameObject);

            var turnOrder = CombatManager.Instance.TurnOrder;

            if (turnOrder == null || turnOrder.Count < 1)
            {
                return;
            }

            _sprites = new Queue<GameObject>();


            int count = 0;
            foreach (var entity in turnOrder)
            {
                if (count >= MaxSprites)
                {
                    break;
                }

                var instance = Instantiate(TurnOrderSpritePrefab, new Vector3(0, 0), Quaternion.identity);

                Sprite sprite;
                if (entity.IsPlayer())
                {
                    sprite = PlayerSprite;
                }
                else
                {
                    sprite = EnemySprite;
                }

                instance.gameObject.GetComponent<Image>().sprite = sprite;

                instance.transform.SetParent(TurnOrderParent);

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
