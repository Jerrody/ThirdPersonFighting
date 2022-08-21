using Characters.Components;
using Characters.Interfaces;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(HealthComponent))]
    public abstract class EntityController : MonoBehaviour, IHealable, IDamageable
    {
        [SerializeField] protected float walkSpeed = 5.0f;
        [SerializeField] protected float runSpeed = 10.0f;

        protected CharacterController Controller { get; private set; }
        private HealthComponent _health;

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
            _health = GetComponent<HealthComponent>();
        }

        public virtual void TakeDamage(uint damageAmount)
        {
            _health.TakeDamage(damageAmount);
        }

        public virtual void HealUp(uint healAmount)
        {
            _health.HealUp(healAmount);
        }

        public bool IsAlive()
        {
            return _health.IsAlive;
        }
    }
}