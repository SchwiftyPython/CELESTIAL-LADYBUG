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

            Schemes = new List<ColorScheme>
            {
                new ColorScheme(Palette.Instance.DarkDesatBlue, Palette.Instance.DarkBlue, Palette.Instance.BurntOrange),
                new ColorScheme(Palette.Instance.DarkLimeGreen, Palette.Instance.DesatBlue, Palette.Instance.DesatOrange),
                new ColorScheme(Palette.Instance.GrayBlue, Palette.Instance.LightGrayBlue, Palette.Instance.ModerateLimeGreen),
                new ColorScheme(Palette.Instance.ModeratePink, Palette.Instance.Orange, Palette.Instance.ModerateOrange)
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
