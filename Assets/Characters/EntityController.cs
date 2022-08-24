using Characters.Components;
using Characters.Interfaces;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(HealthComponent))]
    public abstract class EntityController : MonoBehaviour, IHealable, IDamageable
    {
        [SerializeField] protected float walkSpeed = 5.0f;

        protected HealthComponent Health;

        private void Awake()
        {
            Health = GetComponent<HealthComponent>();
        }

        public virtual void TakeDamage(uint damageAmount)
        {
            Health.TakeDamage(damageAmount);
        }

        public virtual void HealUp(uint healAmount)
        {
            Health.HealUp(healAmount);
        }
    }
}