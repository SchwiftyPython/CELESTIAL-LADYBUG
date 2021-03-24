using UnityEngine;

namespace Assets.Scripts
{
    public class ColorScheme
    {
        public Color RedSwap;
        public Color GreenSwap;
        public Color BlueSwap;

        public ColorScheme(Color redSwap, Color greenSwap, Color blueSwap)
        {
            RedSwap = redSwap;
            GreenSwap = greenSwap;
            BlueSwap = blueSwap;
        }
    }
}
