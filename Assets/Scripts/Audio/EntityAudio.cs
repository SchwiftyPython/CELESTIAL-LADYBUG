using UnityEngine;

namespace Assets.Scripts.Audio
{
    //todo if all the methods can be static then we'll just make this a static class and
    //not bother with attaching to gameobject

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
