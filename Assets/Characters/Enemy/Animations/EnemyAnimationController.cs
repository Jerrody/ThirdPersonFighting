using UnityEngine;

namespace Characters.Enemy.Animations
{
    public sealed class EnemyAnimationController : EntityAnimationController
    {
        [SerializeField] private EnemyController enemy;

        private static readonly int VelocityParam = Animator.StringToHash("Velocity");

        private void Start()
        {
            enemy.OnAttack += () => Anim.SetTrigger(AttackParam);
            enemy.OnDeath += OnDeath;
        }

        private void Update()
        {
            Anim.SetFloat(VelocityParam, enemy.Velocity);
        }

        private void OnDeath()
        {
            enemy.OnDeath -= OnDeath;
            Anim.SetBool(DeadParam, true);
        }
    }
}