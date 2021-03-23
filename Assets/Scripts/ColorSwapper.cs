using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class ColorSwapper : MonoBehaviour
    {
        //private Palette _palette;

        // private void Start()
        // {
        //     SwapColorsOnSprite(new ColorScheme());
        // }

        public void SwapColorsOnSprite(ColorScheme scheme)
        {
            var mat = GetComponent<Renderer>().material;

            mat.SetColor("_ColorSwapRed", scheme.RedSwap);
            mat.SetColor("_ColorSwapGreen", scheme.GreenSwap);
            mat.SetColor("_ColorSwapBlue", scheme.BlueSwap);
        }
    }
}
