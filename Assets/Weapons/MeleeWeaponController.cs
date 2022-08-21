using System;
using Characters.Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace Weapons
{
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
        [Header("References")] [SerializeField]
        private BoxCollider collisionCollider;

        [SerializeField] private BoxCollider hitArea;

        [field: Header("Stats")] [SerializeField]
        private WeaponType weaponType;

        [SerializeField] private uint attackDamage = 1;

        [Header("Transform in hand")] [SerializeField]
        private Vector3 positionInHand = Vector3.zero;

        [SerializeField] private Vector3 rotationInHand = Vector3.zero;

        private Rigidbody _rb;

        private bool _isDropped = true;

        private void OnValidate()
        {
            if (weaponType == WeaponType.Unarmed)
                throw new Exception("Should be not `Unarmed` `WeaponType`!");
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            SetColliders();

            // TODO: Get easy to get a reference to the `Player`.
            // PlayerController.OnAttack.AddListener(OnAttack);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IDamageable>(out var target)) return;

            target.TakeDamage(attackDamage);
            hitArea.gameObject.SetActive(false);
        }

        public Vector3 GetPositionInHands()
        {
            return positionInHand;
        }

        public Quaternion GetRotationInHands()
        {
            return Quaternion.Euler(rotationInHand);
        }

        public WeaponType GetWeaponType()
        {
            return weaponType;
        }

        public void OnPickUp(Transform attachTo)
        {
            _isDropped = false;

            var newTransform = transform;
            newTransform.position = positionInHand;
            newTransform.rotation = Quaternion.Euler(rotationInHand);
            newTransform.SetParent(attachTo, false);

            SetColliders();
        }

        public void OnDrop()
        {
            _isDropped = true;

            transform.SetParent(null);

            SetColliders();
        }

        private void OnAttack()
        {
            hitArea.gameObject.SetActive(true);
        }

        private void SetColliders()
        {
            _rb.isKinematic = !_isDropped;
            collisionCollider.gameObject.SetActive(_isDropped);
            hitArea.gameObject.SetActive(!_isDropped);
        }
    }
}