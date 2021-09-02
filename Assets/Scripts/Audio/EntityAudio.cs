using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class EntityAudio : MonoBehaviour
    {
        public string AttackSound { set; get; }
        public string HurtSound { set; get; }

        public void Attack(string soundPath)
        {
            if (string.IsNullOrEmpty(soundPath))
            {
                Debug.LogWarning("No attack sound defined!");
                return;
            }

            PlaySound(soundPath);
        }

        public void Attack()
        {
            if (string.IsNullOrEmpty(AttackSound))
            {
                Debug.LogWarning("No attack sound defined!");
                return;
            }

            PlaySound(AttackSound);
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

        public void TakeDamage()
        {
            if (string.IsNullOrEmpty(HurtSound))
            {
                Debug.LogWarning("No hurt sound defined!");
                return;
            }

            PlaySound(HurtSound);
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
