using UnityEngine;

namespace Characters.Animation
{
    public delegate void AnimationNotify();

    public sealed class AnimationEventListener : MonoBehaviour
    {
        public event AnimationNotify OnAttackAnimationEnd;
        public event AnimationNotify OnDeathAnimationEnd;

        public void AttackAnimationEnd()
        {
            OnAttackAnimationEnd?.Invoke();
        }

        public void DeathAnimationEnd()
        {
            OnDeathAnimationEnd?.Invoke();
        }
    }
}