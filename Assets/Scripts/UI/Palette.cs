using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Palette : MonoBehaviour
    {
        private Dictionary<string, Color> _colors;

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

            PopulateColorDictionary();
        }

        public void PopulateColorDictionary()
        {
            _colors = new Dictionary<string, Color>
            {
                { "strong red", StrongRed },
                { "moderate orange", ModerateOrange },
                { "very soft orange", VerySoftOrange },
                { "soft orange", SoftOrange },
                { "burnt orange", BurntOrange },
                { "dark moderate red", DarkModerateRed },
                { "very dark desat pink", VeryDarkDesatPink },
                { "dark red", DarkRed },
                { "bright red", BrightRed },
                { "vivid orange", VividOrange },
                { "bright orange", BrightOrange },
                { "soft yellow", SoftYellow },
                { "moderate lime green", ModerateLimeGreen },
                { "dark lime green", DarkLimeGreen },
                { "very dark lime green", VeryDarkLimeGreen },
                { "very dark desat cyan", VeryDarkDesatCyan },
                { "dark blue", DarkBlue },
                { "pure blue", PureBlue },
                { "bright cyan", BrightCyan },
                { "white", White },
                { "light gray blue", LightGrayBlue },
                { "gray blue", GrayBlue },
                { "dark desat blue", DarkDesatBlue },
                { "very dark desat blue", VeryDarkDesatBlue },
                { "desat blue", DesatBlue },
                { "very dark blue", VeryDarkBlue },
                { "pure pink", PurePink },
                { "very dark desat magenta", VeryDarkDesatMagenta },
                { "moderate pink", ModeratePink },
                { "soft red", SoftRed },
                { "orange", Orange },
                { "desat orange", DesatOrange }
            };
        }

        public Color? GetColor(string colorName)
        {
            if (!_colors.ContainsKey(colorName))
            {
                Debug.LogError($"Color {colorName} not found!");

                return null;
            }

            return _colors[colorName];
        }
    }
}
