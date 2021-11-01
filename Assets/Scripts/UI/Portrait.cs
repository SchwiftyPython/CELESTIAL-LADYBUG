using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.UI
{
    public class Portrait : MonoBehaviour
    {
        public enum Slot
        {
            Skin, 
            //Ears,
            Chest,
            FacialHair,
            Hair,
            //Helmet
        }

        [SerializeField] private GameObject _skin;
        [SerializeField] private GameObject _ears;
        [SerializeField] private GameObject _chest;
        [SerializeField] private GameObject _facialHair;
        [SerializeField] private GameObject _hair;
        [SerializeField] private GameObject _helmet;

        public void SetSprite(Slot slot, Sprite sprite)
        {
            switch (slot)
            {
                case Slot.Skin:
                    if (sprite == null)
                    {
                        _skin.gameObject.SetActive(false);
                    }
                    else
                    {
                        _skin.GetComponent<Image>().sprite = sprite;
                    }
                    break;
                // case Slot.Ears:
                //     _ears.GetComponent<Image>().sprite = sprite;
                //     break;
                case Slot.Chest:
                    if (sprite == null)
                    {
                        _chest.gameObject.SetActive(false);
                    }
                    else
                    {
                        _chest.GetComponent<Image>().sprite = sprite;
                    }
                    break;
                case Slot.FacialHair:
                    if (sprite == null)
                    {
                        _facialHair.gameObject.SetActive(false);
                    }
                    else
                    {
                        _facialHair.GetComponent<Image>().sprite = sprite;
                    }
                    break;
                case Slot.Hair:
                    if (sprite == null)
                    {
                        _hair.gameObject.SetActive(false);
                    }
                    else
                    {
                        _hair.GetComponent<Image>().sprite = sprite;
                    }
                    break;
                // case Slot.Helmet:
                //     _helmet.GetComponent<Image>().sprite = sprite;
                //     break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public void SetPortrait(Dictionary<Slot, Sprite> sprites)
        {
            foreach (Slot slot in Enum.GetValues(typeof(Slot)))
            {
                if (!sprites.ContainsKey(slot))
                {
                    SetSprite(slot, null);
                    continue;
                }

                var sprite = sprites[slot];

                SetSprite(slot, sprite);
            }
        }

        public void MakeRandomPortrait()
        {
            const int helmetChance = 47;

            if (Random.Range(0, 101) > helmetChance)
            {
                _helmet.SetActive(true);
            }
            else
            {
                _helmet.SetActive(false);
            }

            foreach (Slot slot in Enum.GetValues(typeof(Slot)))
            {
                var spriteStore = Object.FindObjectOfType<SpriteStore>();
                var sprite = spriteStore.GetRandomSpriteForSlot(slot);

                SetSprite(slot, sprite);
            }
        }
    }
}
