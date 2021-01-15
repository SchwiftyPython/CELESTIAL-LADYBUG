using UnityEngine;

namespace Assets.Scripts
{
    //todo refactor
    public class EntityPrefabStore : MonoBehaviour
    {
        public GameObject CompanionPrototypePrefab;
        public GameObject EnemyPrototypePrefab;
        public GameObject DerpusPrototypePrefab;

        public static EntityPrefabStore Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
