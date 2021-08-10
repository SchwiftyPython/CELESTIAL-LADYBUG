using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class MusicController : MonoBehaviour
    {
        [FMODUnity.EventRef] public string battleMusic;

        private FMOD.Studio.EventInstance _musicInstance;

        public void PlayBattleMusic()
        {
            _musicInstance = FMODUnity.RuntimeManager.CreateInstance(battleMusic);

            _musicInstance.start();
        }

        public void PlayBattleVictoryMusic()
        {
            _musicInstance.setParameterByName("Victory", 1f);
        }

        public void PlayBattleGameOverMusic()
        {
            _musicInstance.setParameterByName("Game Over", 1f);
        }
    }
}
