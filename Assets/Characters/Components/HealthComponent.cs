using Unity.Mathematics;
using UnityEngine;

namespace Characters.Components
{
    public delegate void HealthNotify(float totalHealth, float currentHealthAmount);

    // TODO: Write tests.
    public sealed class HealthComponent : MonoBehaviour
    {
        public event HealthNotify OnHealthIncrease;
        public event HealthNotify OnHealthDecrease;

        [SerializeField] private int healthAmount = 100;

        public bool IsAlive => healthAmount > 0;
        private int _totalHealth;

        private void OnValidate()
        {
            if (healthAmount < 0)
                healthAmount = 0;
        }

        private void Awake()
        {
            _totalHealth = healthAmount;
        }

        public void TakeDamage(uint damageAmount)
        {
            healthAmount = math.clamp(healthAmount - (int)damageAmount, 0, _totalHealth);
            OnHealthDecrease?.Invoke(_totalHealth, healthAmount);
        }

        public void HealUp(uint healAmount)
        {
            if (!IsAlive) return;
            
            healthAmount = math.clamp(healthAmount + (int)healAmount, 0, _totalHealth);
            OnHealthIncrease?.Invoke(_totalHealth, healthAmount);
        }
    }
}