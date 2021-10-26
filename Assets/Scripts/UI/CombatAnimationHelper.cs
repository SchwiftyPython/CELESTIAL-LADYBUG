using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AI;
using Assets.Scripts.Audio;
using Assets.Scripts.Entities;
using DG.Tweening;
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
            else
            {
                StartCoroutine(Target.PlayDeathAnimation());
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

        public void PlayDeathSound()
        {
            var eAudio = GetComponent<EntityAudio>();

            eAudio.Die();
        }

        public void StartSmoothMove(Tween pathTween)
        {
            StartCoroutine(SmoothMove(pathTween));
        }

        public IEnumerator SmoothMove(Tween pathTween)
        {
            yield return SmoothMoveCr(pathTween);
        }

        private IEnumerator SmoothMoveCr(Tween pathTween)
        {
            var ai = GetComponent<AiController>();

            if (ai != null)
            {
                ai.animating = true;
            }

            yield return pathTween.WaitForCompletion();

            if (ai != null)
            {
                yield return Delay();
                ai.animating = false;
            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.00f);
        }
    }
}
