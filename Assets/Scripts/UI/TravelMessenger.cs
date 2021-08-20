using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Utilities.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TravelMessenger : MonoBehaviour
    {
        private const int MaxMessagesOnScreen = 5;

        private Queue<GameObject> _messagesOnScreen;

        public GameObject messagePrefab;

        public RectTransform messageParent;

        public ScrollRect messageScrollRect;

        public GameObject portraitParent;

        private void Start()
        {
            _messagesOnScreen = new Queue<GameObject>();
        }

        public void PartyMessage(string message)
        {
            portraitParent.SetActive(false);

            ClearExcessOnScreenMessages();

            var messageInstance = Instantiate(messagePrefab, messagePrefab.transform.position, Quaternion.identity);

            messageInstance.transform.SetParent(messageParent);

            var rect = messageInstance.GetComponent<RectTransform>();

            rect.localScale = new Vector3(1, 1, 1);

            rect.sizeDelta = new Vector2(1705, rect.sizeDelta.y);

            var writer = GetComponent<TextWriter>();

            writer.AddWriter(messageInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), message,
                GlobalHelper.DefaultTextSpeed, true);

            _messagesOnScreen.Enqueue(messageInstance);

            StartCoroutine(PushToBottom());
        }

        public void EntityMessage(string message, Entity companion)
        {
            SetPortrait(companion.Portrait);

            ClearExcessOnScreenMessages();

            var messageInstance = Instantiate(messagePrefab, messagePrefab.transform.position, Quaternion.identity);

            messageInstance.transform.SetParent(messageParent);

            var rect = messageInstance.GetComponent<RectTransform>();

            rect.localScale = new Vector3(1, 1, 1);

            var writer = GetComponent<TextWriter>();

            writer.AddWriter(messageInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), message,
                GlobalHelper.DefaultTextSpeed, true);

            _messagesOnScreen.Enqueue(messageInstance);

            StartCoroutine(PushToBottom());

        }

        private void SetPortrait(Dictionary<Portrait.Slot, string> portraitKeys)
        {
            var sprites = new Dictionary<Portrait.Slot, Sprite>();

            var spriteStore = FindObjectOfType<SpriteStore>();

            foreach (var slot in portraitKeys.Keys)
            {
                var slotSprite = spriteStore.GetPortraitSpriteForSlotByKey(slot, portraitKeys[slot]);
                sprites.Add(slot, slotSprite);
            }

            var portrait = portraitParent.GetComponent<Portrait>();

            portrait.SetPortrait(sprites);
        }

        private void ClearExcessOnScreenMessages()
        {
            while (_messagesOnScreen.Count >= MaxMessagesOnScreen)
            {
                var messageToClear = _messagesOnScreen.Dequeue();
                Destroy(messageToClear);
            }
        }

        private void ClearAllOnScreenMessages()
        {
            foreach (var messageObject in _messagesOnScreen)
            {
                Destroy(messageObject);
            }

            _messagesOnScreen = new Queue<GameObject>();
        }

        private IEnumerator PushToBottom()
        {
            yield return new WaitForEndOfFrame();
            messageScrollRect.verticalNormalizedPosition = 0;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)messageScrollRect.transform);
        }
    }
}
