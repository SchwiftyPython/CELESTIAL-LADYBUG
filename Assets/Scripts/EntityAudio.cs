using UnityEngine;

namespace Assets.Scripts
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
            var hitSound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
            hitSound.start();

            //FMODUnity.RuntimeManager.PlayOneShot(soundPath, transform.position);
        }

        public void Die()
        {
            //todo get die sound from entity
        }
    }
}
