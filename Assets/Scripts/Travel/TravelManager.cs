using UnityEngine;

namespace Assets.Scripts.Travel
{
    public class TravelManager : MonoBehaviour
    {
        public Party Party { get; private set; }

        public static TravelManager Instance;

        private void Awake()
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
    }
}
