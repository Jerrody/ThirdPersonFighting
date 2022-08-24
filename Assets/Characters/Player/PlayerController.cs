using Characters.Animation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapons;

namespace Characters.Player
{
    public delegate WeaponType WeaponManipulations(int slotIndex);

    public delegate void PlayerNotify();

    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerController : EntityController
    {
        // TODO: Make it unserializable and move it to the `WeaponHolderController`.
        public event WeaponManipulations OnWeaponSwitch;
        public UnityEvent<WeaponType> onWeaponSwitched;
        public event PlayerNotify OnAttack;
        public UnityEvent<InputAction.CallbackContext> onBlock;
        public UnityEvent onWeaponDrop;
        public UnityEvent<MeleeWeaponController> onWeaponTake;

        [SerializeField] private AnimationEventListener animEventListener; // TODO: Move it to the `EntityController`?
        private const float GravityScale = -0.5f;

        public Vector3 Velocity { get; private set; }

        private CharacterController Controller { get; set; }
        private MeleeWeaponController _takeableWeapon;

        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _inputMoveAxis = Vector2.zero;
        private int _attackVariants; // SHOULD IT BE HERE?

        // TODO: Remove it and make `enabled` as an indicator?
        private bool _canSwitchWeapon = true;

        private void Start()
        {
            Controller = GetComponent<CharacterController>();
            animEventListener.OnAttackAnimationEnd += OnAttackEnd;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer != Layers.Weapon || hit.collider.isTrigger) return;

            _takeableWeapon = hit.gameObject.GetComponentInParent<MeleeWeaponController>();
        }

        private void Update()
        {
            _moveDirection = new Vector3(Velocity.x, GravityScale, Velocity.z);
            _moveDirection = transform.TransformDirection(_moveDirection);

            Controller.Move(_moveDirection * Time.deltaTime);
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
            if (!context.started || !_canSwitchWeapon || !enabled) return;

            var numKey = int.Parse(context.control.name);

            var newActiveWeaponType = (WeaponType)OnWeaponSwitch?.Invoke(numKey - 1);
            onWeaponSwitched?.Invoke(newActiveWeaponType);

            enabled = true;
        }

        public void PickUp(InputAction.CallbackContext context)
        {
            if (!context.started || _takeableWeapon == null || !_canSwitchWeapon) return;

            onWeaponTake?.Invoke(_takeableWeapon);
            onWeaponSwitched?.Invoke(_takeableWeapon.WeaponType);
            _takeableWeapon = null;

            enabled = true;
        }

        public void Drop(InputAction.CallbackContext context)
        {
            if (!context.started || !_canSwitchWeapon) return;

            onWeaponDrop?.Invoke();
            onWeaponSwitched?.Invoke(WeaponType.Unarmed); // TODO: Return `WeaponType` by the event.

            enabled = true;
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (!context.started || OnAttack == null) return;

            enabled = false;
            OnAttack.Invoke();

            _canSwitchWeapon = enabled;
        }

        public void Block(InputAction.CallbackContext context)
        {
            onBlock?.Invoke(context);
            enabled = context.canceled;
        }

        private void OnAttackEnd()
        {
            enabled = true;
            _canSwitchWeapon = true;
        }
    }
}