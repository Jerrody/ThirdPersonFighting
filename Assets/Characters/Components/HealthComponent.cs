using Unity.Mathematics;
using UnityEngine;

namespace Characters.Components
{
    // TODO: Write tests.
    public sealed class HealthComponent : MonoBehaviour
    {
        [SerializeField, Range(0, int.MaxValue)]
        private int healthAmount = 100;

        public bool IsAlive => healthAmount >= 0;
        private int _totalHealth;

        private void Awake()
        {
            _totalHealth = healthAmount;
        }

        public void TakeDamage(uint damageAmount)
        {
            healthAmount = math.clamp(healthAmount - (int)damageAmount, 0, _totalHealth);
        }

        public void HealUp(uint healAmount)
        {
            if (IsAlive)
                healthAmount = math.clamp(healthAmount - (int)healAmount, 0, _totalHealth);
        }
    }
}