using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapons;

namespace Characters.Player
{
    public delegate WeaponType WeaponManipulations(int slotIndex);

    public sealed class PlayerController : EntityController
    {
        // TODO: Make it unserializable.
        public event WeaponManipulations OnWeaponSwitch;
        public UnityEvent<WeaponType> onWeaponSwitched;
        public UnityEvent onAttack;
        public UnityEvent onWeaponDrop;
        public UnityEvent<MeleeWeaponController> onWeaponTake;

        private const float GravityScale = -0.5f;

        public Vector3 Velocity { get; private set; }

        private MeleeWeaponController _takeableWeapon;

        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _inputMoveAxis = Vector2.zero;

        private void Update()
        {
            _moveDirection = new Vector3(Velocity.x, GravityScale, Velocity.z);
            _moveDirection = transform.TransformDirection(_moveDirection);

            Controller.Move(_moveDirection * Time.deltaTime);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != Layers.Weapon || other.isTrigger) return;

            _takeableWeapon = other.GetComponentInParent<MeleeWeaponController>();
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != Layers.Weapon || other.isTrigger) return;

            _takeableWeapon = null;
        }

        public void Move(InputAction.CallbackContext context)
        {
            _inputMoveAxis = context.ReadValue<Vector2>();

            Velocity = new Vector3(_inputMoveAxis.x * walkSpeed, _moveDirection.y, _inputMoveAxis.y * walkSpeed);
        }

        public void SwitchWeapons(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            var numKey = int.Parse(context.control.name);

            var newActiveWeaponType = (WeaponType)OnWeaponSwitch?.Invoke(numKey - 1);
            onWeaponSwitched?.Invoke(newActiveWeaponType);
        }

        public void PickUp(InputAction.CallbackContext context)
        {
            if (!context.started || _takeableWeapon == null) return;

            onWeaponTake?.Invoke(_takeableWeapon);
            onWeaponSwitched?.Invoke(_takeableWeapon.GetWeaponType());
            _takeableWeapon = null;
        }

        public void Drop(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            onWeaponDrop?.Invoke();
            onWeaponSwitched?.Invoke(WeaponType.Unarmed);
        }
    }
}