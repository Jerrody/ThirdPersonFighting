using UnityEngine;

namespace Characters.Player.Animations
{
    public delegate void AnimationNotify();

    public sealed class AnimationEventListener : MonoBehaviour
    {
        public event AnimationNotify OnAttackAnimationEnd;

        public void AttackAnimationEnd()
        {
            OnAttackAnimationEnd?.Invoke();
        }
    }
}