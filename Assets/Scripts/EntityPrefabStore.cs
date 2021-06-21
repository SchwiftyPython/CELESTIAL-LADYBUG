using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Companions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class EntityPrefabStore : MonoBehaviour
    {
        private readonly List<Race.RaceType> _companionRaces = new List<Race.RaceType>
        {
            Race.RaceType.Human,
            Race.RaceType.Dwarf,
            Race.RaceType.Elf,
            Race.RaceType.Gnome,
            Race.RaceType.Halfling
        };

        private readonly Dictionary<Type, Func<Race.RaceType, bool, Entity>> _companions = new Dictionary<Type, Func<Race.RaceType, bool, Entity>>
        {
            {typeof(Crossbowman), (rType, isPlayer)  => new Crossbowman(rType, isPlayer)},
            {typeof(Gladiator), (rType, isPlayer)  => new Gladiator(rType, isPlayer)},
            {typeof(Knight), (rType, isPlayer)  => new Knight(rType, isPlayer)},
            {typeof(ManAtArms), (rType, isPlayer)  => new ManAtArms(rType, isPlayer)},
            {typeof(Paladin), (rType, isPlayer)  => new Paladin(rType, isPlayer)},
            {typeof(Spearman), (rType, isPlayer)  => new Spearman(rType, isPlayer)},
            {typeof(Wizard), (rType, isPlayer)  => new Wizard(rType, isPlayer)},
        };

        //todo organize collections by group - necromancer, castle, etc

        private Dictionary<string, GameObject> _prefabDictionary;

        public GameObject[] combatSpritePrefabs;

        private void Awake()
        {
            _prefabDictionary = PopulateDictionaryFromArray(combatSpritePrefabs);
        }

        public GameObject GetCombatSpritePrefab(string prefabName)
        {
            if (_prefabDictionary == null || !_prefabDictionary.ContainsKey(prefabName.ToLower()))
            {
                Debug.LogError($"Combat sprite for {prefabName} does not exist!");
                return null;
            }

            return _prefabDictionary[prefabName.ToLower()];
        }

        public Entity GetRandomCompanion()
        {
            var index = Random.Range(0, _companions.Count);

            var key = _companions.ElementAt(index).Key;

            var rType = _companionRaces[Random.Range(0, _companionRaces.Count)];

            return _companions[key].Invoke(rType, true);
        }

        public Entity GetCompanion(Type companionType)
        {
            if (!_companions.ContainsKey(companionType))
            {
                return null;
            }

            var rType = _companionRaces[Random.Range(0, _companionRaces.Count)];

            return _companions[companionType].Invoke(rType, true);
        }

        private static Dictionary<string, GameObject> PopulateDictionaryFromArray(IEnumerable<GameObject> prefabs)
        {
            var prefabDictionary = new Dictionary<string, GameObject>();

            foreach (var prefab in prefabs)
            {
                prefabDictionary.Add(prefab.name.ToLower(), prefab);
            }

            return prefabDictionary;
        }
    }
}
