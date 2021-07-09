using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    //todo replace with fmod
    //todo need more generic attack sound event where sound played is based on equipped weapon or ability 
    public class AudioManager : MonoBehaviour, ISubscriber
    {
        private readonly IList<string> _subscribedEvents = new List<string>
        {
            GlobalHelper.ButtonClick,
            GlobalHelper.MeleeHit,
            GlobalHelper.MeleeMiss
        };

        public AudioClip Click;
        public AudioClip MeleeHit;
        public AudioClip MeleeMiss;
        public AudioClip CompanionPain;
        public AudioClip EnemyPain;

        public AudioSource SoundSource;

        private void Start()
        {
            SubscribeToEvents();
        }

        private void PlayOneShot(AudioClip clip)
        {
            if (SoundSource == null)
            {
                SoundSource = gameObject.GetComponent<AudioSource>();
            }

            SoundSource.PlayOneShot(clip, .5f);
        }

        private void SubscribeToEvents()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            foreach (var eventName in _subscribedEvents)
            {
                eventMediator.SubscribeToEvent(eventName, this);
            }
        }

        private void UnsubscribeFromEvents()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator?.UnsubscribeFromAllEvents(this);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(GlobalHelper.ButtonClick))
            {
                PlayOneShot(Click);
            }
            else if (eventName.Equals(GlobalHelper.MeleeHit))
            {
                PlayOneShot(MeleeHit);

                if (!(broadcaster is Entity attacker))
                {
                    return;
                }

                //todo get corresponding sounds from a sound store
                if (attacker.IsPlayer())
                {
                    PlayOneShot(EnemyPain);
                }
                else
                {
                    PlayOneShot(CompanionPain);
                }
            }
            else if (eventName.Equals(GlobalHelper.MeleeMiss))
            {
                PlayOneShot(MeleeMiss);
            }
        }
    }
}
