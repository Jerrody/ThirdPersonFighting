using System;
using Characters.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public delegate void WeaponAttackAnimationNotify(uint animationIndex);

    public delegate void WeaponBlockAnimationNotify(bool isBlocking);

    public enum WeaponType
    {
        Unarmed = 1,
        GreatSword = 2,
        Axe = 3,
        Sword = 4,
    }

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Rigidbody))]
    public class MeleeWeaponController : MonoBehaviour
    {
        public event WeaponAttackAnimationNotify OnAttackAnimation;
        public event WeaponBlockAnimationNotify OnBlockAnimation;

        [Header("References")]
        [SerializeField] private BoxCollider collisionCollider;
        [SerializeField] private BoxCollider hitArea;

        [Header("Stats")]
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private uint attackDamage = 1;
        [SerializeField] private uint numberOfAttackVariants = 1;

        [Header("Transform in hand")]
        [SerializeField] private Vector3 positionInHand = Vector3.zero;
        [SerializeField] private Vector3 rotationInHand = Vector3.zero;

        public WeaponType WeaponType => weaponType;
        protected Rigidbody Rb;

        private void OnValidate()
        {
            if (weaponType == WeaponType.Unarmed)
                throw new Exception("Should be not `Unarmed` `WeaponType`!");
        }

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();

            SetColliders(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IDamageable>(out var target) || other.gameObject.layer == Layers.Player) return;

            hitArea.gameObject.SetActive(false);
            target.TakeDamage(attackDamage);
        }

        public virtual void OnPickUp(Transform attachTo)
        {
            var newTransform = transform;
            newTransform.position = positionInHand;
            newTransform.rotation = Quaternion.Euler(rotationInHand);
            newTransform.SetParent(attachTo, false);

            SetColliders(false);
        }

        public virtual void OnDrop()
        {
            transform.SetParent(null);

            SetColliders(true);
        }

        public void OnAttack()
        {
            OnAttackAnimation?.Invoke((uint)Random.Range(0, numberOfAttackVariants));

            hitArea.gameObject.SetActive(true);
        }

        public void OnBlock(bool isBlocking)
        {
            collisionCollider.gameObject.SetActive(isBlocking);
            OnBlockAnimation?.Invoke(isBlocking);
        }

        public void OnAttackEnd()
        {
            hitArea.gameObject.SetActive(false);
        }

        private void SetColliders(bool isDropped)
        {
            Rb.isKinematic = !isDropped;
            collisionCollider.gameObject.SetActive(isDropped);
            hitArea.gameObject.SetActive(false);
        }
    }
}