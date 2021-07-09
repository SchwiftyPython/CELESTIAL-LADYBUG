using UnityEngine;

namespace Assets.Scripts
{

    public class ColorSwapper : MonoBehaviour
    {
        public enum ColorSwapSlot 
        {
            Helmet,
            Head,
            Body,
            Hands,
            Feet,
            Weapon
        }

        public ColorSwapSlot swapSlot;

        public void SwapColorsOnSprite(ColorScheme scheme)
        {
            var mat = GetComponent<Renderer>().material;

            mat.SetColor("_ColorSwapRed", scheme.RedSwap);
            mat.SetColor("_ColorSwapGreen", scheme.GreenSwap);
            mat.SetColor("_ColorSwapBlue", scheme.BlueSwap);
        }
    }
}
