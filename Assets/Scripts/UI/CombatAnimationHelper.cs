using Assets.Scripts.Audio;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CombatAnimationHelper : MonoBehaviour
    {
        public Entity Parent;
        public Entity Target;
        public bool attackHit;

        public void StopAttackAnimation()
        {
            Parent.PlayIdleAnimation();
        }

        public void TriggerTargetAnimation()
        {
            if (!attackHit || Target == null)
            {
                return;
            }

            if (!Target.IsDead())
            {
                Target.PlayHitAnimation();
            }
        }

        public void PlayAttackSound()
        {
            var eAudio = GetComponent<EntityAudio>();

            eAudio.Attack();
        }

        public void PlayHitSound()
        {
            var eAudio = GetComponent<EntityAudio>();

            eAudio.TakeDamage();
        }

        public void StopHitAnimation()
        {
            Parent.PlayIdleAnimation();
        }
    }
}
