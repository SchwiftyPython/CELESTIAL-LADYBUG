using Assets.Scripts.Decks;
using UnityEngine;

namespace Assets.Scripts.Encounters
{
    public class EncounterManager : MonoBehaviour, ISubscriber
    {
        private const string EncounterFinished = GlobalHelper.EncounterFinished;

        private bool _timerPaused;

        private EncounterDeck _deck;

        public float TimeTilNextEncounter;

        //todo need ui references 

        public static EncounterManager Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject); //todo if continuity flops we can probably not worry about having a new deck every time

            _deck = new EncounterDeck();

            ResetTimer();
        }

        private void Update()
        {
            if (!_timerPaused)
            {
                if (TimeTilNextEncounter > 0)
                {
                    TimeTilNextEncounter -= Time.deltaTime;
                }
                else
                {
                    PauseTimer();

                    DrawNextEncounter();
                }
            }
        }

        private void DrawNextEncounter()
        {
            var encounter = _deck.Draw();

            encounter.Run();

            EventMediator.Instance.SubscribeToEvent(EncounterFinished, this);
        }

        private void PauseTimer()
        {
            _timerPaused = true;
        }

        private void ResumeTimer()
        {
            _timerPaused = false;
        }

        private void ResetTimer()
        {
            TimeTilNextEncounter = Random.Range(7, 11);
            ResumeTimer();
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(EncounterFinished))
            {
                ResetTimer();

                EventMediator.Instance.UnsubscribeFromEvent(EncounterFinished, this);
            }

            //todo events for pause timer, resume timer, reset timer, draw trigger encounter
        }
    }
}
