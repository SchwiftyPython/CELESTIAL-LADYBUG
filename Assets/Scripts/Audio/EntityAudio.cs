using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class EntityAudio : MonoBehaviour
    {
        public void Attack()
        {
            //todo get sound for equipped weapon from entity
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

            var hitSound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
            hitSound.start();
        }

        public void Die(string soundPath)
        {
            if (string.IsNullOrEmpty(soundPath))
            {
                Debug.LogWarning("No die sound defined!");
                return;
            }

            var dieSound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
            dieSound.start();
        }
    }
}
