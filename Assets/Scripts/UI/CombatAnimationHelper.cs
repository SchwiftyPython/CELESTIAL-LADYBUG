using Assets.Scripts.Audio;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CombatAnimationHelper : MonoBehaviour
    {
        public Entity Parent;

        public void StopAttackAnimation()
        {
            Parent.PlayIdleAnimation();
        }

        public void PlayAttackSound()
        {
            var eAudio = GetComponent<EntityAudio>();

            eAudio.Attack();
        }
    }
}
