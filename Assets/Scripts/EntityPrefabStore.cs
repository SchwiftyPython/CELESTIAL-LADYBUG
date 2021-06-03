using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class EntityPrefabStore : MonoBehaviour
    {
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
                Debug.LogError($"Ability sprite for {prefabName} does not exist!");
                return null;
            }

            return _prefabDictionary[prefabName.ToLower()];
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
