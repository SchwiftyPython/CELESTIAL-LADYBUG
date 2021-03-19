using System;
using Assets.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class SpriteStore : MonoBehaviour
    {
        //todo dictionaries for each slot type

        //for testing
        public Sprite[] SkinSprites;
        public Sprite[] EarSprites;
        public Sprite[] ChestSprites;
        public Sprite[] FacialFairSprites;
        public Sprite[] HairSprites;
        public Sprite[] HelmetSprites;

        public static SpriteStore Instance;

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

            //todo initialize dictionaries
        }

        public Sprite GetRandomSpriteForSlot(Portrait.Slot slot)
        {
            switch (slot)
            {
                case Portrait.Slot.Skin:
                    return SkinSprites[Random.Range(0, SkinSprites.Length)];
                case Portrait.Slot.Ears:
                    return EarSprites[Random.Range(0, EarSprites.Length)]; 
                case Portrait.Slot.Chest:
                    return ChestSprites[Random.Range(0, ChestSprites.Length)];
                case Portrait.Slot.FacialHair:
                    return FacialFairSprites[Random.Range(0, FacialFairSprites.Length)];
                case Portrait.Slot.Hair:
                    return HairSprites[Random.Range(0, HairSprites.Length)];
                case Portrait.Slot.Helmet:
                    return HelmetSprites[Random.Range(0, HelmetSprites.Length)];
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }
    }
}
