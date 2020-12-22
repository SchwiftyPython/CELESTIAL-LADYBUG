using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Encounters
{
    //todo refactor - do we need separate dictionaries for each kind of encounter? Prob triggered (mental break and continuity) and non-triggered at minimum
    //todo we can see how maintainable, debuggable, and performant just performing queries is though
    public class EncounterStore : MonoBehaviour
    {
        private Dictionary<string, Func<Encounter>> _allEncounters;

        private Dictionary<string, Func<Encounter>> _nonTriggeredEncounters = new Dictionary<string, Func<Encounter>>
        {
            {"stay in school", () => new StayInSchool()}
        };

        private void Start()
        {
        
        }

        private void Update()
        {
        
        }

        public List<Encounter> GetAllNonTriggeredEncounters()
        {
            var encounters = new List<Encounter>();

            foreach (var encounter in _allEncounters)
            {
                encounters.Add(encounter.Value.Invoke());
            }

            return encounters;
        }
    }
}
