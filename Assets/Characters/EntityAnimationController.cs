using UnityEngine;

namespace Characters
{
    public abstract class EntityAnimationController : MonoBehaviour
    {
        protected static readonly int AttackParam = Animator.StringToHash("Attack");

        protected Animator Anim;

        private void Awake()
        {
            Anim = GetComponent<Animator>();
        }
    }
}