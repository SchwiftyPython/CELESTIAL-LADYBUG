using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Decks;
using Assets.Scripts.Entities;
using Assets.Scripts.Saving;
using Assets.Scripts.Travel;
using UnityEngine;

namespace Assets.Scripts.Encounters
{
    public class EncounterManager : MonoBehaviour, ISubscriber, ISaveable
    {
        private const string EncounterFinished = GlobalHelper.EncounterFinished;
        private const string MentalBreak = GlobalHelper.MentalBreak;
        private const string PauseTimerEvent = GlobalHelper.PauseTimer;
        private const string ResumeTimerEvent = GlobalHelper.ResumeTimer;

        private bool _timerPaused;
        private bool _encounterInProgress;

        private EncounterDeck _normalEncounterDeck;
        private EncounterDeck _campingDeck;
        private EncounterDeck _testDeck;

        private Queue<Encounter> _encounterQueue;

        public float TimeTilNextEncounter;

        public bool UseTestDeck;

        private void Start()
        {
            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(MentalBreak, this);
            eventMediator.SubscribeToEvent(GlobalHelper.DerpusNoEnergy, this);
            eventMediator.SubscribeToEvent(PauseTimerEvent, this);
            eventMediator.SubscribeToEvent(ResumeTimerEvent, this);

            PauseTimer();
        }

        private void Update()
        {
            if (!_timerPaused && !GameManager.Instance.AnyActiveWindows())
            {
                if (TimeTilNextEncounter > 0)
                {
                    TimeTilNextEncounter -= Time.deltaTime;
                }
                else
                {
                    PauseTimer();

                    var parallax = FindObjectOfType<Parallax>();

                    if (parallax == null)
                    {
                        return;
                    }

                    parallax.Stop();

                    var eventMediator = FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(PauseTimerEvent, this);

                    DrawNextEncounter();
                }
            }
        }

        public void BuildDecksForNewDay()
        {
            var encounterStore = FindObjectOfType<EncounterStore>();

            if (UseTestDeck)
            {
                BuildTestDeck();
            }
            else
            {
                var normalEncounterSize = 3;

                const int extraEncounterChance = 5;

                //todo diceroller
                var roll = Random.Range(1, 101);

                if (roll <= extraEncounterChance)
                {
                    normalEncounterSize++;
                }

                _normalEncounterDeck = new EncounterDeck(encounterStore.GetNormalEncounters(), normalEncounterSize);

                var combatEncounters = encounterStore.GetCombatEncounters();

                _normalEncounterDeck.AddCard(combatEncounters[Random.Range(0, combatEncounters.Count)]);

                _normalEncounterDeck.Shuffle();
            }

            if (_campingDeck == null || _campingDeck.Size < 1)
            {
                _campingDeck = new EncounterDeck(encounterStore.GetCampingEncounters(), 5);
            }

            if (_encounterInProgress)
            {
                return;
            }

            ResetTimer();
            ResumeTimer();
        }

        private void BuildDecksOnLoadGame(EncounterManagerDto emDto)
        {
            var encounterStore = FindObjectOfType<EncounterStore>();

            var normalEncounterSize =  emDto.NormalDeck.Size;

            if (normalEncounterSize > 0)
            {
                _normalEncounterDeck = new EncounterDeck(encounterStore.GetNormalEncounters(), normalEncounterSize);
                _normalEncounterDeck.Shuffle();
            }

            _campingDeck = new EncounterDeck(encounterStore.GetCampingEncounters(), emDto.CampingDeck.Size);

            if (GameManager.Instance.InCombat())
            {
                PauseTimer();
            }
            else
            {
                ResetTimer();
                ResumeTimer();
            }
        }

        private void BuildTestDeck()
        {
            var encounterStore = FindObjectOfType<EncounterStore>();

            var testEncounters = encounterStore.GetTestEncounters();

            _testDeck = new EncounterDeck(testEncounters, testEncounters.Count);

            _testDeck.Shuffle();
        }

        private void DrawNextEncounter()
        {
            Encounter encounter;

            if (UseTestDeck)
            {
                encounter = _testDeck.Draw();
            }
            else
            {
                encounter = _normalEncounterDeck.Draw();
            }

            if (encounter == null)
            {
                encounter = _campingDeck.Draw();
            }

            _encounterInProgress = true;

            encounter.Run();

            var eventMediator = FindObjectOfType<EventMediator>();

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

        public void RunQueuedEncounters()
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

        public IEnumerator RunNextQueuedEncounter()
        {
            var travelManager = FindObjectOfType<TravelManager>();

            if (travelManager.Party.PartyDead())
            {
                var eventMediator = FindObjectOfType<EventMediator>();

                eventMediator.Broadcast(GlobalHelper.GameOver, this);
            }
            else if (_encounterQueue == null || _encounterQueue.Count < 1)
            {
                ResetTimer();

                var eventMediator = FindObjectOfType<EventMediator>();

                _encounterInProgress = false;

                eventMediator.Broadcast(ResumeTimerEvent, this);
            }
            else
            {
                yield return StartCoroutine(Delay());

                var parallax = FindObjectOfType<Parallax>();

                parallax.Stop();

                var encounter = _encounterQueue.Dequeue();

                _encounterInProgress = true;

                encounter.Run();
            }
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
            TimeTilNextEncounter = Random.Range(6, 8);
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.5f);
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            var encounterStore = FindObjectOfType<EncounterStore>();

            if (eventName.Equals(MentalBreak))
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

        private struct EncounterManagerDto
        {
            public EncounterDeck.EncounterDeckDto NormalDeck;
            public EncounterDeck.EncounterDeckDto CampingDeck;
        }

        public object CaptureState()
        {
            if (_normalEncounterDeck == null || _campingDeck == null)
            {
                return new EncounterManagerDto();
            }

            var dto = new EncounterManagerDto
            {
                NormalDeck = (EncounterDeck.EncounterDeckDto)_normalEncounterDeck.CaptureState(),
                CampingDeck = (EncounterDeck.EncounterDeckDto)_campingDeck.CaptureState()
            };

            return dto;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }

            var emDto = (EncounterManagerDto)state;

            BuildDecksOnLoadGame(emDto);
        }
    }
}
