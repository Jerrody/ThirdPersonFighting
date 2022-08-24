using Characters.Interfaces;
using UnityEngine;

namespace Characters.Components
{
    public sealed class HitAreaComponent : MonoBehaviour
    {
        [SerializeField] private uint attackDamage;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IDamageable>(out var damageable) ||
                other.gameObject.layer == Layers.Enemy) return;

            gameObject.SetActive(false);
            damageable.TakeDamage(attackDamage);
        }
    }
}