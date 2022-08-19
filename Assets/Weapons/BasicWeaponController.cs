using UnityEngine;

namespace Weapons
{
    public enum WeaponType
    {
        Unarmed = 1,
        Sword = 2,
        Axe = 3,
        Bow = 4,
    }
    
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BasicWeaponController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected BoxCollider collisionCollider;

        [field: Header("Stats")]
        public WeaponType WeaponType { get; protected set; }
        [SerializeField] protected uint attackDamage = 1;
        [SerializeField] protected bool isDropped;

        [Header("Transform in hand")]
        [SerializeField] protected Vector3 positionInHand = Vector3.zero;
        [SerializeField] protected Quaternion rotationInHand = Quaternion.identity;

        private Rigidbody _rb;

        private void Awake()
        {
            collisionCollider.gameObject.SetActive(isDropped);
            _rb.useGravity = isDropped;
        }

        public Vector3 GetPositionInHands()
        {
            return positionInHand;
        }

        public Quaternion GetRotationInHands()
        {
            return rotationInHand;
        }
    }
}