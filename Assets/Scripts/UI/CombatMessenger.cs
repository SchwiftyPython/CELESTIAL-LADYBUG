﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CombatMessenger : MonoBehaviour, ISubscriber
    {
        private const int MaxMessagesOnScreen = 90;

        private Queue<GameObject> _messagesOnScreen;

        public GameObject MessagePrefab;

        public RectTransform MessageParent;

        public ScrollRect MessageScrollRect;

        private void Start()
        {
            _messagesOnScreen = new Queue<GameObject>();

            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.SubscribeToEvent(GlobalHelper.SendMessageToConsole, this);
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
            MessageScrollRect.verticalNormalizedPosition = 0;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)MessageScrollRect.transform);
        }

        private void CreateOnScreenMessage(string myMessage)
        {
            ClearExcessOnScreenMessages();

            var messageInstance = Instantiate(MessagePrefab, MessagePrefab.transform.position, Quaternion.identity);

            messageInstance.transform.SetParent(MessageParent);

            messageInstance.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            messageInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = myMessage;

            _messagesOnScreen.Enqueue(messageInstance);

            StartCoroutine(PushToBottom());
        }


        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.SendMessageToConsole))
            {
                var message = parameter as string;

                if (string.IsNullOrEmpty(message))
                {
                    Debug.LogError("Empty message passed to CombatMessenger!");
                    return;
                }

                CreateOnScreenMessage(message);
            }
        }
    }
}
