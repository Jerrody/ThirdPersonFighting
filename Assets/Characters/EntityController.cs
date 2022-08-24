using Characters.Components;
using Characters.Interfaces;
using UnityEngine;

namespace Characters
{
    public delegate void EntityNotify();

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
            if (!Health.IsAlive) return; // TODO: Make an event from this?

            Health.TakeDamage(damageAmount);
        }

        public virtual void HealUp(uint healAmount)
        {
            Health.HealUp(healAmount);
        }
    }
}