using Assets.Scripts.Audio;
using FMOD.Studio;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utilities.UI
{
    public class TextWriter : MonoBehaviour
    {
        private EventMediator _eventMediator;
        private EventInstance _sound;

        private string _textWritingSound;

        private TextMeshProUGUI _uiText;
        private string _textToWrite;
        private int _characterIndex;
        private float _timePerCharacter;
        private float _timer;
        private bool _invisibleCharacters;

        public void AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
        {
            if (_eventMediator == null)
            {
                _eventMediator = FindObjectOfType<EventMediator>();
            }

            if (string.IsNullOrEmpty(_textWritingSound))
            {
                var audioStore = FindObjectOfType<AudioStore>();

                _textWritingSound = audioStore.textWriting;
            }

            _uiText = uiText;
            _textToWrite = textToWrite;
            _timePerCharacter = timePerCharacter;
            _invisibleCharacters = invisibleCharacters;
            _characterIndex = 0;

            _sound = FMODUnity.RuntimeManager.CreateInstance(_textWritingSound);
            _sound.start();
        }

        private void Update()
        {
            if (_uiText != null)
            {
                _timer -= Time.deltaTime;

                while (_timer <= 0f)
                {
                    _timer += _timePerCharacter;
                    _characterIndex++;
                    string text = _textToWrite.Substring(0, _characterIndex);

                    if (_invisibleCharacters)
                    {
                        text += "<color=#00000000>" + _textToWrite.Substring(_characterIndex) + "</color>";
                    }

                    _uiText.text = text;

                    if (_characterIndex >= _textToWrite.Length)
                    {
                        _uiText = null;
                        _eventMediator.Broadcast(GlobalHelper.WritingFinished, this);
                        _sound.stop(STOP_MODE.IMMEDIATE);
                        return;
                    }
                }
            }
        }

        public void DisplayMessageInstantly()
        {
            _timePerCharacter = 0f;
        }
    }
}
