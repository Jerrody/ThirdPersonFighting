using UnityEngine;

namespace Characters
{
    public abstract class EntityAnimationController : MonoBehaviour
    {
        protected Animator Anim;

        private void Awake()
        {
            Anim = GetComponent<Animator>();
        }
    }
}