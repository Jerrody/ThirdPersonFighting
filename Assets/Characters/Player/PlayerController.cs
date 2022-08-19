using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapons;

namespace Characters.Player
{
    public sealed class PlayerController : EntityController
    {
        public UnityEvent<int> OnWeaponSwitch;
        public UnityEvent OnWeaponSwitched;
        public UnityEvent OnAttack;
        public UnityEvent OnWeaponDrop;
        public UnityEvent<BasicWeaponController> OnWeaponTake;

        private const float GravityScale = -0.5f;

        public WeaponType WeaponType { get; private set; } = WeaponType.Unarmed;
        public Vector3 Velocity { get; private set; }

        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _inputMoveAxis = Vector2.zero;

        private void Update()
        {
            _moveDirection = new Vector3(Velocity.x, GravityScale, Velocity.z);
            _moveDirection = transform.TransformDirection(_moveDirection);

            Controller.Move(_moveDirection * Time.deltaTime);
        }

        public void Move(InputAction.CallbackContext context)
        {
            _inputMoveAxis = context.ReadValue<Vector2>();

            Velocity = new Vector3(_inputMoveAxis.x * walkSpeed, _moveDirection.y, _inputMoveAxis.y * walkSpeed);
        }

        public void SwitchWeapons(InputAction.CallbackContext context)
        {
            int.TryParse(context.control.name, out var numKey);

            OnWeaponSwitch?.Invoke(numKey - 1);
        }
    }
}