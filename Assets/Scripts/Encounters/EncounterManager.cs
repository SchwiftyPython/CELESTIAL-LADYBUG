using System.Collections.Generic;
using Assets.Scripts.Decks;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Encounters
{
    public class EncounterManager : MonoBehaviour, ISubscriber
    {
        private const string EncounterFinished = GlobalHelper.EncounterFinished;
        private const string MentalBreak = GlobalHelper.MentalBreak;
        private const string PauseTimerEvent = GlobalHelper.PauseTimer;
        private const string ResumeTimerEvent = GlobalHelper.ResumeTimer;

        private bool _timerPaused;
        private bool _encounterInProgress;

        private EncounterDeck _normalEncounterDeck;
        private EncounterDeck _campingDeck;

        private Queue<Encounter> _encounterQueue;

        public float TimeTilNextEncounter;

        //public static EncounterManager Instance;

        private void Start()
        {
            // if (Instance == null)
            // {
            //     Instance = this;
            // }
            // else if (Instance != this)
            // {
            //     Destroy(gameObject);
            // }
            // DontDestroyOnLoad(gameObject);

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(MentalBreak, this);
            eventMediator.SubscribeToEvent(GlobalHelper.DerpusNoEnergy, this);
            eventMediator.SubscribeToEvent(PauseTimerEvent, this);
            eventMediator.SubscribeToEvent(ResumeTimerEvent, this);

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

            var encounterStore = Object.FindObjectOfType<EncounterStore>();

            _normalEncounterDeck = new EncounterDeck(encounterStore.GetNormalEncounters(), normalEncounterSize);

            var combatEncounters = encounterStore.GetCombatEncounters();

            _normalEncounterDeck.AddCard(combatEncounters[Random.Range(0, combatEncounters.Count)]);

            _normalEncounterDeck.Shuffle();

            if (_campingDeck == null || _campingDeck.Size < 1)
            {
                _campingDeck = new EncounterDeck(encounterStore.GetCampingEncounters(), 5);
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

            _encounterInProgress = true;

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(EncounterFinished, this);
        }

        private void AddToEncounterQueue(Encounter encounter)
        {
            if (_encounterQueue == null)
            {
                _encounterQueue = new Queue<Encounter>();
            }

            _encounterQueue.Enqueue(encounter);
        }

        private void RunQueuedEncounters()
        {
            if (_encounterQueue != null && _encounterQueue.Count > 0)
            {
                foreach (var encounter in _encounterQueue)
                {
                    encounter.Run();
                }
            }

            _encounterQueue?.Clear();
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
            var encounterStore = Object.FindObjectOfType<EncounterStore>();

            if (eventName.Equals(EncounterFinished))
            {
                ResetTimer();

                var eventMediator = Object.FindObjectOfType<EventMediator>();

                eventMediator.UnsubscribeFromEvent(EncounterFinished, this);

                RunQueuedEncounters();

                _encounterInProgress = false;
            }
            else if (eventName.Equals(MentalBreak))
            {
                if(!(broadcaster is Entity companion))
                {
                    return;
                }

                if (companion.IsDerpus())
                {
                    AddToEncounterQueue(encounterStore.GetDerpusNoMoraleEncounter());
                }
                else
                {
                    //todo draw from the mental break deck
                    AddToEncounterQueue(encounterStore.GetRandomMentalBreakEncounter(companion));
                }
            }
            else if (eventName.Equals(GlobalHelper.DerpusNoEnergy))
            {
                AddToEncounterQueue(encounterStore.GetDerpusNoEnergyEncounter());
            }
            else if (eventName.Equals(PauseTimerEvent))
            {
                PauseTimer();
            }
            else if (eventName.Equals(ResumeTimerEvent))
            {
                if (_encounterInProgress)
                {
                    return;
                }

                ResumeTimer();
            }

            //todo events for reset timer, draw trigger encounter
        }
    }
}
