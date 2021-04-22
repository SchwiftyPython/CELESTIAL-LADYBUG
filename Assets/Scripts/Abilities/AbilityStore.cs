using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class AbilityStore : MonoBehaviour
    {
        private Dictionary<string, Ability> _allAbilities;

        public static AbilityStore Instance;

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
        }

        public Ability GetAbilityByName(string abilityName)
        {
            if (!_allAbilities.ContainsKey(abilityName))
            {
                Debug.LogError($"Ability {abilityName} does not exist!");
                return null;
            }

            return _allAbilities[abilityName];
        }
    }
}
