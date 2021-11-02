using Assets.Scripts.Utilities.Save_Load;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SettingsWindow : MonoBehaviour, ISaveable
    {
        [SerializeField] private GameObject uiContainer = null;
        [SerializeField] private Slider masterVolumeSlider = null;
        [SerializeField] private Slider musicVolumeSlider = null;
        [SerializeField] private Slider soundEffectsVolumeSlider = null;
        [SerializeField] private Toggle fullScreenToggle = null;

        private FMOD.Studio.Bus _masterBus;
        private FMOD.Studio.Bus _musicBus;
        private FMOD.Studio.Bus _soundBus;

        private bool _tutorialsEnabled;

        private void Start()
        {
            if (uiContainer.activeSelf)
            {
                Hide();
            }

            _masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
            _musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
            _soundBus = FMODUnity.RuntimeManager.GetBus("bus:/SoundEffects");
            _tutorialsEnabled = true;

            var savingSystem = FindObjectOfType<SavingSystem>();

            savingSystem.LoadSettings();
        }

        public void Show()
        {
            uiContainer.SetActive(true);
            GameManager.Instance.AddActiveWindow(uiContainer);
        }

        public void Hide()
        {
            uiContainer.SetActive(false);
            GameManager.Instance.RemoveActiveWindow(uiContainer);
        }

        public void SetMusicVolume(float volume)
        {
            _musicBus.setVolume(volume);
        }

        public void SetSoundVolume(float volume)
        {
            _soundBus.setVolume(volume);
        }

        public void SetMasterVolume(float volume)
        {
            _masterBus.setVolume(volume);
        }

        public void ToggleFullscreen(bool state)
        {
            Screen.fullScreen = state;
        }

        public void SaveSettings()
        {
            var savingSystem = FindObjectOfType<SavingSystem>();

            savingSystem.SaveSettings();
        }

        public void EnableTutorials()
        {
            _tutorialsEnabled = true;
        }

        public void DisableTutorials()
        {
            _tutorialsEnabled = false;
        }

        private struct SettingsDto
        {
            public float MasterVolume;
            public float MusicVolume;
            public float SoundEffectsVolume;
            public bool FullScreen;
            public bool TutorialsEnabled;
        }

        public object CaptureState()
        {
            var dto = new SettingsDto();

            _masterBus.getVolume(out dto.MasterVolume);
            _musicBus.getVolume(out dto.MusicVolume);
            _soundBus.getVolume(out dto.SoundEffectsVolume);
            dto.FullScreen = Screen.fullScreen;
            dto.TutorialsEnabled = _tutorialsEnabled;

            return dto;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }

            var dto = (SettingsDto)state;

            SetMasterVolume(dto.MasterVolume);
            masterVolumeSlider.value = dto.MasterVolume;

            SetMusicVolume(dto.MusicVolume);
            musicVolumeSlider.value = dto.MusicVolume;

            SetSoundVolume(dto.SoundEffectsVolume);
            soundEffectsVolumeSlider.value = dto.SoundEffectsVolume;

            ToggleFullscreen(dto.FullScreen);
            fullScreenToggle.isOn = dto.FullScreen;

            _tutorialsEnabled = dto.TutorialsEnabled;
        }
    }
}
