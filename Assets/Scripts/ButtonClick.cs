using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts
{
    public class ButtonClick : MonoBehaviour
    {
        private string _buttonClick;
        private string _buttonHover;

        private void Start()
        {
            var audioStore = FindObjectOfType<AudioStore>();

            _buttonClick = audioStore.buttonClick;
            _buttonHover = audioStore.buttonHover;
        }

        public void Clicked()
        {
            var sound = FMODUnity.RuntimeManager.CreateInstance(_buttonClick);
            sound.start();
        }

        public void OnHover()
        {
            var sound = FMODUnity.RuntimeManager.CreateInstance(_buttonHover);
            sound.start();
        }
    }
}
