using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class ColorThemeRascal : MonoBehaviour
    {
        public List<ColorScheme> Schemes;

        public GameObject BigThiccyBodyBoi;
        public GameObject BigThiccyHands;
        public GameObject BigThiccyHelmet;
        public GameObject BigThiccyFeet;
        public GameObject BigThiccyHead;
        public GameObject BigThiccyWeapon;

        private int _index;

        private void Start()
        {
            _index = 0;

            var palette = FindObjectOfType<Palette>();

            Schemes = new List<ColorScheme>
            {
                new ColorScheme(palette.DarkDesatBlue, palette.DarkBlue, palette.BurntOrange),
                new ColorScheme(palette.DarkLimeGreen, palette.DesatBlue, palette.DesatOrange),
                new ColorScheme(palette.GrayBlue, palette.LightGrayBlue, palette.ModerateLimeGreen),
                new ColorScheme(palette.ModeratePink, palette.Orange, palette.ModerateOrange)
            };

            ApplyColorScheme(Schemes.First());
        }

        public void ApplyColorScheme(ColorScheme scheme)
        {
            BigThiccyBodyBoi.GetComponent<ColorSwapper>().SwapColorsOnSprite(scheme);
            BigThiccyHands.GetComponent<ColorSwapper>().SwapColorsOnSprite(scheme);
            BigThiccyHelmet.GetComponent<ColorSwapper>().SwapColorsOnSprite(scheme);
            BigThiccyFeet.GetComponent<ColorSwapper>().SwapColorsOnSprite(scheme);
            BigThiccyHead.GetComponent<ColorSwapper>().SwapColorsOnSprite(scheme);
            BigThiccyWeapon.GetComponent<ColorSwapper>().SwapColorsOnSprite(scheme);
        }

        public void SelectNextColorScheme()
        {
            if (_index + 1 >= Schemes.Count || _index < 0)
            {
                _index = 0;
            }
            else
            {
                _index++;
            }

            ApplyColorScheme(Schemes[_index]);
        }
    }
}
