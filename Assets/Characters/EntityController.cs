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

        protected CharacterController Controller;
        protected HealthComponent Health;

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
            Health = GetComponent<HealthComponent>();
        }

        public void TakeDamage(uint damageAmount)
        {
            Health.TakeDamage(damageAmount);
        }

        public void HealUp(uint healAmount)
        {
            Health.HealUp(healAmount);
        }

        public bool IsAlive()
        {
            return Health.IsAlive;
        }
    }
}