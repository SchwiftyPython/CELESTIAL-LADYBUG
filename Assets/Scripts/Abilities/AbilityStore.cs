using System;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class AbilityStore : MonoBehaviour
    {
        private Dictionary<string, Func<Entity, Ability>> _allAbilities = new Dictionary<string, Func<Entity, Ability>>
        {
            {"tank", abilityOwner => new Tank(abilityOwner)},
            {"well fed", abilityOwner => new WellFed(abilityOwner)},
            {"riposte", abilityOwner => new Riposte(abilityOwner)}
        };


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

        public Ability GetAbilityByName(string abilityName, Entity abilityOwner)
        {
            abilityName = abilityName.ToLower();

            if (!_allAbilities.ContainsKey(abilityName))
            {
                Debug.LogError($"Ability {abilityName} does not exist!");
                return null;
            }

            return _allAbilities[abilityName].Invoke(abilityOwner);
        }
    }
}
