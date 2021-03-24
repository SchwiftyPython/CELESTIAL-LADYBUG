using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Palette : MonoBehaviour
    {
        public Color StrongRed;
        public Color ModerateOrange;
        public Color VerySoftOrange;
        public Color SoftOrange;
        public Color BurntOrange;
        public Color DarkModerateRed;
        public Color VeryDarkDesatPink;
        public Color DarkRed;
        public Color BrightRed;
        public Color VividOrange;
        public Color BrightOrange;
        public Color SoftYellow;
        public Color ModerateLimeGreen;
        public Color DarkLimeGreen;
        public Color VeryDarkLimeGreen;
        public Color VeryDarkDesatCyan;
        public Color DarkBlue;
        public Color PureBlue;
        public Color BrightCyan;
        public Color White;
        public Color LightGrayBlue;
        public Color GrayBlue;
        public Color DarkDesatBlue;
        public Color VeryDarkDesatBlue;
        public Color DesatBlue;
        public Color VeryDarkBlue;
        public Color PurePink;
        public Color VeryDarkDesatMagenta;
        public Color ModeratePink;
        public Color SoftRed;
        public Color Orange;
        public Color DesatOrange;

        public static Palette Instance;

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
