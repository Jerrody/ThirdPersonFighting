using Characters.Interfaces;
using Characters.Player;
using UnityEngine;

namespace Weapons.Melee
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class MeleeWeaponController : BasicWeaponController
    {
        [Header("References")] [SerializeField]
        protected BoxCollider hitArea;

        [SerializeField] protected PlayerController player;
        
        private void Awake()
        {
            hitArea.gameObject.SetActive(false);

            player.OnAttack.AddListener(OnAttack);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IDamageable>(out var target)) return;

            target.TakeDamage(attackDamage);
            hitArea.gameObject.SetActive(false);
        }

        private void OnAttack()
        {
            hitArea.gameObject.SetActive(true);
        }
    }
}