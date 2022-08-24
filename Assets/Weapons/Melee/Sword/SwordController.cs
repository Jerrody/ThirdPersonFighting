using UnityEngine;

namespace Weapons.Melee.Sword
{
    public class SwordController : MeleeWeaponController
    {
        [SerializeField] private BoxCollider swordBoxCollider;
        [SerializeField] private GameObject shield;

        [SerializeField] private Vector3 shieldPositionInHand;
        [SerializeField] private Vector3 shieldRotationInHand;

        [SerializeField] private Transform attachShieldTo;

        private Rigidbody _shieldRigidBody;

        private void Start()
        {
            _shieldRigidBody = shield.GetComponent<Rigidbody>();
        }

        public override void OnPickUp(Transform attachTo)
        {
            var newTransform = shield.gameObject.transform;

            newTransform.SetParent(attachShieldTo);
            newTransform.localPosition = shieldPositionInHand;
            newTransform.localRotation = Quaternion.Euler(shieldRotationInHand);
            base.OnPickUp(attachTo);

            swordBoxCollider.gameObject.SetActive(false);
            _shieldRigidBody.isKinematic = true;
        }

        public override void OnDrop()
        {
            base.OnDrop();

            shield.gameObject.transform.SetParent(transform);
            swordBoxCollider.gameObject.SetActive(true);
            _shieldRigidBody.isKinematic = false;
        }

        private void OnEnable()
        {
            shield.SetActive(true);
        }

        private void OnDisable()
        {
            shield.SetActive(false);
        }
    }
}