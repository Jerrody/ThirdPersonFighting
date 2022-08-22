using System;
using Characters.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public delegate void WeaponAnimationNotify(uint animationIndex);

    public enum WeaponType
    {
        Unarmed = 1,
        Sword = 2,
        Axe = 3,
        Glaive = 4,
    }

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class MeleeWeaponController : MonoBehaviour
    {
        // Shouldn't be static.
        public event WeaponAnimationNotify OnAttackAnimation;

        [Header("References")] [SerializeField]
        private BoxCollider collisionCollider;

        [SerializeField] private BoxCollider hitArea;

        [Header("Stats")] [SerializeField] private WeaponType weaponType;
        [SerializeField] private uint attackDamage = 1;

        [SerializeField] private uint numberOfAttackVariants = 1;

        [Header("Transform in hand")] [SerializeField]
        private Vector3 positionInHand = Vector3.zero;

        [SerializeField] private Vector3 rotationInHand = Vector3.zero;

        private Rigidbody _rb;

        private void OnValidate()
        {
            if (weaponType == WeaponType.Unarmed)
                throw new Exception("Should be not `Unarmed` `WeaponType`!");
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            SetColliders(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IDamageable>(out var target) || other.gameObject.layer == Layers.Player) return;

            target.TakeDamage(attackDamage);
            hitArea.gameObject.SetActive(false);
        }

        public WeaponType GetWeaponType()
        {
            return weaponType;
        }

        public void OnPickUp(Transform attachTo)
        {
            var newTransform = transform;
            newTransform.position = positionInHand;
            newTransform.rotation = Quaternion.Euler(rotationInHand);
            newTransform.SetParent(attachTo, false);

            SetColliders(false);
        }

        public void OnDrop()
        {
            transform.SetParent(null);

            SetColliders(true);
        }

        public void OnAttack()
        {
            OnAttackAnimation?.Invoke((uint)Random.Range(0, numberOfAttackVariants));
            hitArea.gameObject.SetActive(true);
        }

        public void OnBlock()
        {
            // TODO: Make a block.
        }

        public void OnAttackEnd()
        {
            hitArea.gameObject.SetActive(false);
        }

        private void SetColliders(bool isDropped)
        {
            _rb.isKinematic = !isDropped;
            collisionCollider.gameObject.SetActive(isDropped);
            hitArea.gameObject.SetActive(false);
        }
    }
}