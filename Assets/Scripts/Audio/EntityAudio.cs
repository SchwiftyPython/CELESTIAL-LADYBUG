using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class EntityAudio : MonoBehaviour 
    {
        public void Attack(string soundPath)
        {
            if (string.IsNullOrEmpty(soundPath))
            {
                Debug.LogWarning("No attack sound defined!");
                return;
            }

            PlaySound(soundPath);
        }

        public void CastSpell()
        {
            //todo get sound for spell
        }

        public void TakeDamage(string soundPath)
        {
            if (string.IsNullOrEmpty(soundPath))
            {
                Debug.LogWarning("No hurt sound defined!");
                return;
            }

            PlaySound(soundPath);
        }

        public void Die(string soundPath)
        {
            if (string.IsNullOrEmpty(soundPath))
            {
                Debug.LogWarning("No die sound defined!");
                return;
            }

            PlaySound(soundPath);
        }

        private void PlaySound(string soundPath)
        {
            var sound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
            sound.start();
        }
    }
}
