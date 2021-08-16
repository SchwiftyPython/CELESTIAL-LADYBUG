using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class MusicController : MonoBehaviour
    {
        [FMODUnity.EventRef] public string battleMusic;
        [FMODUnity.EventRef] public string travelMusic;
        [FMODUnity.EventRef] public string titleMusic;

        [SerializeField] private bool muteMusic;

        private FMOD.Studio.EventInstance _musicInstance;

        private void Awake()
        {
            if (muteMusic)
            {
                MuteMusic();
            }
        }

        public void PlayBattleMusic()
        {
            PlayMusic(battleMusic);
        }

        public void EndBattleMusic()
        {
            _musicInstance.setParameterByName("End Battle Music", 1f);
        }

        public void PlayBattleVictoryMusic()
        {
            _musicInstance.setParameterByName("Victory", 1f);
        }

        public void PlayBattleGameOverMusic()
        {
            _musicInstance.setParameterByName("Game Over", 1f);
        }

        public void PlayTravelMusic()
        {
            PlayMusic(travelMusic);
        }

        public void EndTravelMusic()
        {
            _musicInstance.setParameterByName("End Travel Music", 1f);
        }

        public void PlayTitleMusic()
        {
            PlayMusic(titleMusic);
        }

        public void EndTitleMusic()
        {
            _musicInstance.setParameterByName("End Title Music", 1f);
        }

        public void MuteMusic()
        {
            _musicInstance.setParameterByName("Mute", 1f);
        }

        private void PlayMusic(string path)
        {
            _musicInstance = FMODUnity.RuntimeManager.CreateInstance(path);

            _musicInstance.start();

            if (muteMusic)
            {
                MuteMusic();
            }
        }
    }
}
