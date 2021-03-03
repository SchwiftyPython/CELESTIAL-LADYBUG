using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Encounters.Camping;
using Assets.Scripts.Encounters.Combat;
using Assets.Scripts.Encounters.MentalBreak;
using Assets.Scripts.Encounters.Normal;
using Assets.Scripts.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Encounters
{
    //todo refactor - do we need separate dictionaries for each kind of encounter? Prob triggered (mental break and continuity) and non-triggered at minimum
    //todo we can see how maintainable, debuggable, and performant just performing queries is though
    //todo since we're starting to assign encounters to their own decks, would make sense to see if we can load these from a file
    public class EncounterStore : MonoBehaviour
    {
        private readonly Dictionary<string, Func<Encounter>> _normalEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"stay in school", () => new StayInSchool()},
            {"fight or flight", () => new FightOrFlight()},
            {"genie in a bottle", () => new GenieInABottle()},
            {"sweetroll robbery", () => new SweetrollRobbery()},
            {"disabled wagon", () => new DisabledWagon()},
            {"nasty storm", () => new NastyStorm()},
            {"rock skipping contest", () => new RockSkippingContest()},
            {"cauldron roulette", () => new CauldronRoulette()}
        };

        private readonly Dictionary<string, Func<Encounter>> _campingEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"camp mosquito", () => new CampMosquito()},
            {"comfy inn", () => new ComfyInn()},
            {"holy inferno", () => new HolyInferno()},
            {"star blanket", () => new StarBlanket()},
            {"conk out", () => new ConkOut()},
            {"peaceful village", () => new PeacefulVillage()}
        };

        private readonly Dictionary<string, Func<Encounter>> _combatEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"bandit attack", () => new BanditAttack()}
        };

        private readonly Dictionary<string, Func<Entity, Encounter>> _mentalBreakEncounters = new Dictionary<string, Func<Entity, Encounter>>
        {
            {"dazed mugging", dazedCompanion => new DazedMugging(dazedCompanion)}
        };

        public static EncounterStore Instance;

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

            // _allEncounters = new Dictionary<string, Func<Encounter>>();
            //
            // foreach (var encounter in _normalEncounters)
            // {
            //     _allEncounters.Add(encounter.Key, encounter.Value);
            // }
            //
            // foreach (var encounter in _campingEncounters)
            // {
            //     _allEncounters.Add(encounter.Key, encounter.Value);
            // }
        }

        public List<Encounter> GetNormalEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _normalEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }

        public List<Encounter> GetCampingEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _campingEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }

        public List<Encounter> GetCombatEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _combatEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }

        public Encounter GetRandomMentalBreakEncounter(Entity companion)
        {
            var index = Random.Range(0, _mentalBreakEncounters.Count);

            var key = _mentalBreakEncounters.ElementAt(index).Key;

            return _mentalBreakEncounters[key].Invoke(companion);
        }
    }
}
