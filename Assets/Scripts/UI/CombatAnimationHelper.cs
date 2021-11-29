using System.Collections;
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
        public int damage;
        public bool criticalHit;

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

            Target.PlayImpactNoise();

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

        public void ShakeSprite()
        {
            transform.DOShakePosition(0.4f, new Vector3(.02f, .02f));
        }

        public void FlashSprite()
        {
            SpriteRenderer uiImage = GetComponent<SpriteRenderer>();
            
            if (uiImage == null)
            {
                uiImage = GetComponentInChildren<SpriteRenderer>();
            }
            
            uiImage.material = new Material(uiImage.material);
            
            StartCoroutine(FlashSpriteCr(uiImage.material));
        }

        private IEnumerator FlashSpriteCr(Material mat)
        {
            if (mat == null)
            {
                yield return null;
            }

            yield return mat.DOFloat(0.08f, "_HitEffectBlend", 0.1f).WaitForCompletion();

            yield return mat.DOFloat(0.0f, "_HitEffectBlend", 0.1f).WaitForCompletion();

            yield return mat.DOFloat(0.08f, "_HitEffectBlend", 0.1f).WaitForCompletion();

            yield return mat.DOFloat(0.0f, "_HitEffectBlend", 0.1f).WaitForCompletion();
        }

        public void ShowDamagePopup()
        {
            DamagePopup.Create(transform.position, damage, criticalHit);
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

            yield return pathTween.WaitForCompletion();
        }

        public IEnumerator Delay(float time)
        {
            yield return new WaitForSeconds(time);
        }
    }
}
