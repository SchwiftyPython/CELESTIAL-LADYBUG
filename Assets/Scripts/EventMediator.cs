using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class EventMediator : MonoBehaviour
    {
        private Dictionary<string, List<ISubscriber>> _eventSubscriptions;

        private void Awake()
        {
            _eventSubscriptions = new Dictionary<string, List<ISubscriber>> ();
        }

        public void SubscribeToEvent(string eventName, ISubscriber subscriber)
        {
            if (_eventSubscriptions == null)
            {
                _eventSubscriptions = new Dictionary<string, List<ISubscriber>>();
            }

            if (_eventSubscriptions.ContainsKey(eventName))
            {
                _eventSubscriptions[eventName].Add(subscriber);
            }
            else
            {
                _eventSubscriptions.Add(eventName, new List<ISubscriber> {subscriber});
            }
        }

        public void UnsubscribeFromEvent(string eventName, ISubscriber subscriber)
        {
            if (!_eventSubscriptions.ContainsKey(eventName))
            {
                return;
            }

            _eventSubscriptions[eventName].Remove(subscriber);
        }

        public void Broadcast(string eventName, object broadcaster, object parameter = null)
        {
            if (!_eventSubscriptions.ContainsKey(eventName))
            {
                return;
            }

            var subscribers = _eventSubscriptions[eventName];

            foreach (var sub in subscribers.ToArray())
            {
                try
                {
                    NotifySubscriber(eventName, broadcaster, sub, parameter);
                }
                catch (Exception e)
                {
                    subscribers.Remove(sub);
                    Debug.LogWarning("Missing Subscriber: " + e + "\nRemoved from EventMediator.");
                }
            }
        }

        public void UnsubscribeFromAllEvents(ISubscriber subscriber)
        {
            foreach (var subscribers in _eventSubscriptions.Keys.Select(eventName => _eventSubscriptions[eventName])
                .Where(subscribers => subscribers.Contains(subscriber)))
            {
                subscribers.Remove(subscriber);
            }
        }

        private static void NotifySubscriber(string eventName, object broadcaster, ISubscriber subscriber, object parameter = null)
        {
            subscriber.OnNotify(eventName, broadcaster, parameter);
        }
    }
}
