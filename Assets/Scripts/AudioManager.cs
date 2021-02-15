using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    //todo need more generic attack sound event where sound played is based on equipped weapon or ability 
    public class AudioManager : MonoBehaviour, ISubscriber
    {
        private readonly IList<string> _subscribedEvents = new List<string>
        {
            GlobalHelper.ButtonClick,
            GlobalHelper.MeleeHit
        };

        public AudioClip Click;
        public AudioClip MeleeHit;

        public AudioSource SoundSource;

        private void Start()
        {
            SubscribeToEvents();
        }

        private void Play()
        {
            SoundSource.Play();
        }

        private void SubscribeToEvents()
        {
            foreach (var eventName in _subscribedEvents)
            {
                EventMediator.Instance.SubscribeToEvent(eventName, this);
            }
        }

        private void UnsubscribeFromEvents()
        {
            EventMediator.Instance.UnsubscribeFromAllEvents(this);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.ButtonClick))
            {
                SoundSource.clip = Click;
                SoundSource.volume = .50f;
                Play();
            }
            else if (eventName.Equals(GlobalHelper.MeleeHit))
            {
                SoundSource.clip = MeleeHit;
                SoundSource.volume = .50f;
                Play();
            }
        }
    }
}
