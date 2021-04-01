using UnityEngine;

namespace Assets.Scripts.Utilities
{
    /// <summary>
    /// An alternative to using the singleton pattern. Will handle spawning a
    /// prefab only once across multiple scenes.
    ///
    /// Place this in a prefab that exists in every scene. Point to another
    /// prefab that contains all GameObjects that should be singletons. The
    /// class will spawn the prefab only once and set it to persist between
    /// scenes.
    /// </summary>
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [Tooltip("This prefab will only be spawned once and persisted between " +
                 "scenes.")]
        [SerializeField]
        private readonly GameObject _persistentObjectPrefab = null;

        private static bool _hasSpawned;

        private void Awake() 
        {
            if (_hasSpawned)
            {
                return;
            }

            SpawnPersistentObjects();

            _hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            var persistentObject = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}