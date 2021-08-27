using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CombatAnimationHelper : MonoBehaviour
    {
        public void StopAttackAnimation()
        {
            var animator = GetComponent<Animator>();

            animator.SetBool("IsAttacking", false);
        }

        public void PlayAttackSound()
        {
            var eAudio = GetComponent<EntityAudio>();

            eAudio.Attack();
        }
    }
}
