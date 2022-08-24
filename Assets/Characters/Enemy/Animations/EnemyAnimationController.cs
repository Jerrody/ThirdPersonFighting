using UnityEngine;

namespace Characters.Enemy.Animations
{
    public sealed class EnemyAnimationController : EntityAnimationController
    {
        [SerializeField] private EnemyController enemy;

        private static readonly int VelocityParam = Animator.StringToHash("Velocity");

        private void Update()
        {
            Anim.SetFloat(VelocityParam, enemy.Velocity);
        }
    }
}