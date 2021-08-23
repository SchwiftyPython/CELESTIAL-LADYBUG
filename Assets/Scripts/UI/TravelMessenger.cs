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
        private struct EntityMessageDto
        {
            public string Message;
            public Dictionary<Portrait.Slot, string> Portrait;
        }

        private const int MaxMessagesOnScreen = 5;

        private Queue<GameObject> _messagesOnScreen;

        private Queue<string> _partyMessageQueue;
        private Queue<EntityMessageDto> _entityMessageQueue;

        private Vector3 _messageStartPosition;
        private Vector3 _messageTarget;

        public GameObject messagePrefab;

        public RectTransform messageParent;

        public ScrollRect messageScrollRect;

        private void Start()
        {
            _messagesOnScreen = new Queue<GameObject>();
        }

        public void DisplayAllMessages()
        {
            StartCoroutine(DisplayMessagesWithDelay());
        }

        private IEnumerator DisplayMessagesWithDelay()
        {
            if (_partyMessageQueue != null && _partyMessageQueue.Count > 0)
            {
                foreach (var message in _partyMessageQueue.ToArray())
                {
                    DisplayPartyMessage(message);
                    yield return StartCoroutine(Delay());
                }
            }

            if (_entityMessageQueue != null && _entityMessageQueue.Count > 0)
            {
                foreach (var message in _entityMessageQueue.ToArray())
                {
                    DisplayEntityMessage(message);
                    yield return StartCoroutine(Delay());
                }
            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
        }

        public void QueuePartyMessages(List<string> messages)
        {
            //todo need to wait until popup is closed -- can probably queue everything up then run when ok button is clicked on result popup
            //todo need delay in between each message and animation
            //todo need to make the travel message one panel deeper so we can animate it and not worry about the parent position

            _partyMessageQueue = new Queue<string>();

            foreach (var message in messages) 
            {
                _partyMessageQueue.Enqueue(message);
            }
        }

        private void DisplayPartyMessage(string message)
        {
            ClearExcessOnScreenMessages();
            var messageInstance = Instantiate(messagePrefab, messagePrefab.transform.position, Quaternion.identity);

            messageInstance.transform.GetChild(0).gameObject.SetActive(false); //portrait parent set to false

            messageInstance.transform.SetParent(messageParent);

            var rect = messageInstance.GetComponent<RectTransform>();

            rect.localScale = new Vector3(1, 1, 1);

            rect.sizeDelta = new Vector2(1705, rect.sizeDelta.y);

            var writer = messageInstance.GetComponent<TextWriter>();

            writer.AddWriter(messageInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), message,
                GlobalHelper.DefaultTextSpeed, true);

            _messagesOnScreen.Enqueue(messageInstance);

            StartCoroutine(PushToBottom());
        }

        public void QueueEntityMessage(string message, Entity companion)
        {
            var emDto = new EntityMessageDto
            {
                Message = message,
                Portrait = companion.Portrait
            };

            if (_entityMessageQueue == null)
            {
                _entityMessageQueue = new Queue<EntityMessageDto>();
            }

            _entityMessageQueue.Enqueue(emDto);
        }

        private void DisplayEntityMessage(EntityMessageDto emDto)
        {
            ClearExcessOnScreenMessages();

            var messageInstance = Instantiate(messagePrefab, messagePrefab.transform.position, Quaternion.identity);

            messageInstance.transform.SetParent(messageParent);

            var rect = messageInstance.GetComponent<RectTransform>();

            rect.localScale = new Vector3(1, 1, 1);

            var portrait = messageInstance.GetComponent<TravelMessage>().portrait;

            SetPortrait(emDto.Portrait, portrait);

            var writer = messageInstance.GetComponent<TextWriter>();

            writer.AddWriter(messageInstance.GetComponent<TravelMessage>().messageText, emDto.Message,
                GlobalHelper.DefaultTextSpeed, true);

            _messagesOnScreen.Enqueue(messageInstance);

            StartCoroutine(PushToBottom());
        }

        public void ClearMessageQueues()
        {
            _partyMessageQueue = new Queue<string>();
            _entityMessageQueue = new Queue<EntityMessageDto>();
        }

        private void SetPortrait(Dictionary<Portrait.Slot, string> portraitKeys, Portrait portrait)
        {
            var sprites = new Dictionary<Portrait.Slot, Sprite>();

            var spriteStore = FindObjectOfType<SpriteStore>();

            foreach (var slot in portraitKeys.Keys)
            {
                var slotSprite = spriteStore.GetPortraitSpriteForSlotByKey(slot, portraitKeys[slot]);
                sprites.Add(slot, slotSprite);
            }

            //var portrait = portraitParent.GetComponent<Portrait>();

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
