using UnityEngine;

namespace Characters
{
    public abstract class EntityAnimationController : MonoBehaviour
    {
        protected static readonly int AttackParam = Animator.StringToHash("Attack");
        protected static readonly int HitParam = Animator.StringToHash("Hit");
        protected static readonly int DeadParam = Animator.StringToHash("Dead");

        protected Animator Anim;

        private void Awake()
        {
            Anim = GetComponent<Animator>();
        }
    }
}