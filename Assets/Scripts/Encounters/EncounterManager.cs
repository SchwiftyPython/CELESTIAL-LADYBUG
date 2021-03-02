using Assets.Scripts.Decks;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Encounters
{
    public class EncounterManager : MonoBehaviour, ISubscriber
    {
        private const string EncounterFinished = GlobalHelper.EncounterFinished;
        private const string MentalBreak = GlobalHelper.MentalBreak;

        private bool _timerPaused;

        private EncounterDeck _normalEncounterDeck;
        private EncounterDeck _campingDeck;

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
            DontDestroyOnLoad(gameObject);
            
            EventMediator.Instance.SubscribeToEvent(MentalBreak, this);

            PauseTimer();
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

        public void BuildDecksForNewDay()
        {
            var normalEncounterSize = 3;

            const int extraEncounterChance = 5;

            //todo diceroller
            var roll = Random.Range(1, 101);

            if (roll <= extraEncounterChance)
            {
                normalEncounterSize++;
            }

            _normalEncounterDeck = new EncounterDeck(EncounterStore.Instance.GetNormalEncounters(), normalEncounterSize);

            if (_campingDeck == null || _campingDeck.Size < 1)
            {
                _campingDeck = new EncounterDeck(EncounterStore.Instance.GetCampingEncounters(), 5);
            }

            ResetTimer();
        }

        private void DrawNextEncounter()
        {
            var encounter = _normalEncounterDeck.Draw();
            
            if (encounter == null)
            {
                encounter = _campingDeck.Draw();
            }

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
            else if (eventName.Equals(MentalBreak))
            {
                if(!(broadcaster is Entity companion))
                {
                    return;
                }

                //todo draw from the mental break deck
                EncounterStore.Instance.GetRandomMentalBreakEncounter(companion).Run();
            }

            //todo events for pause timer, resume timer, reset timer, draw trigger encounter
        }
    }
}
