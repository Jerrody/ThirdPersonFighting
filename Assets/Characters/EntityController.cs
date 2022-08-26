using Characters.Animations;
using Characters.Components;
using Characters.Interfaces;
using UnityEngine;

namespace Characters
{
    public delegate void EntityNotify();

    [RequireComponent(typeof(HealthComponent))]
    public abstract class EntityController : MonoBehaviour, IDamageable
    {
        [Header("References")]
        [SerializeField] protected AnimationEventListener animEventListener;

        [Header("Stats")]
        [SerializeField] protected float moveSpeed = 5.0f;

        protected HealthComponent Health;

        private void Awake()
        {
            Health = GetComponent<HealthComponent>();
        }

        public virtual void TakeDamage(uint damageAmount)
        {
            if (!Health.IsAlive) return;

            Health.TakeDamage(damageAmount);
        }

        protected void HealUp(uint healAmount)
        {
            Health.HealUp(healAmount);
        }
    }
}