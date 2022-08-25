using System;
using Characters.Components;
using Characters.Enemy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Weapons;

namespace Characters.Player
{
    public delegate WeaponType WeaponManipulations(int slotIndex);

    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerController : EntityController
    {
        public event EntityNotify OnEscapePressed;
        public event EntityNotify OnAttack;
        public event EntityNotify OnHit;
        public event EntityNotify OnDeath;
        public event EntityNotify OnWeaponDrop;

        public event WeaponManipulations OnWeaponSwitch;

        public event WeaponOperations OnWeaponTake;

        [field: NonSerialized] public UnityEvent<WeaponType> OnWeaponSwitched;
        [field: NonSerialized] public UnityEvent<InputAction.CallbackContext> OnBlock;

        [SerializeField] private int healAmountOnKill = 40;

        private const float GravityScale = -20.0f;

        public Vector3 Velocity { get; private set; }

        private CharacterController _controller;
        private MeleeWeaponController _takeableWeapon;

        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _inputMoveAxis = Vector2.zero;

        private void OnValidate()
        {
            if (healAmountOnKill < 0)
                healAmountOnKill = 0;
        }

        private void Awake()
        {
            OnWeaponSwitched = new UnityEvent<WeaponType>();
            OnBlock = new UnityEvent<InputAction.CallbackContext>();

            _controller = GetComponent<CharacterController>();
            Health = GetComponent<HealthComponent>();

            animEventListener.OnAttackAnimationEnd += () => { enabled = true; };
            EnemyController.OnKill += () => { HealUp((uint)healAmountOnKill); };
        }

        private void Update()
        {
            _moveDirection = new Vector3(Velocity.x, GravityScale, Velocity.z);
            _moveDirection = transform.TransformDirection(_moveDirection);

            _controller.Move(_moveDirection * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != Layers.Weapon || other.isTrigger) return;

            _takeableWeapon = other.gameObject.GetComponentInParent<MeleeWeaponController>();
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != Layers.Weapon || other.isTrigger) return;

            _takeableWeapon = null;
        }

        public override void TakeDamage(uint damageAmount)
        {
            base.TakeDamage(damageAmount);
            if (!Health.IsAlive)
            {
                enabled = false;

                OnDeath?.Invoke();

                return;
            }

            OnHit?.Invoke();
            enabled = true;
        }

        public void Move(InputAction.CallbackContext context)
        {
            _inputMoveAxis = context.ReadValue<Vector2>();

            Velocity = new Vector3(_inputMoveAxis.x * moveSpeed, _moveDirection.y, _inputMoveAxis.y * moveSpeed);
        }

        public void SwitchWeapons(InputAction.CallbackContext context)
        {
            if (!context.started || !enabled) return;

            enabled = true;

            var numKey = int.Parse(context.control.name);

            // NOTE: It is safe operation if to use only numerics keys and not `0` for binding.
            var newActiveWeaponType = (WeaponType)OnWeaponSwitch?.Invoke(numKey - 1);
            OnWeaponSwitched?.Invoke(newActiveWeaponType);
        }

        public void PickUp(InputAction.CallbackContext context)
        {
            if (!context.started || _takeableWeapon == null || !enabled) return;

            enabled = true;

            if (!(bool)OnWeaponTake?.Invoke(_takeableWeapon)) return;
            OnWeaponSwitched?.Invoke(_takeableWeapon.WeaponType);

            _takeableWeapon = null;
        }

        public void Drop(InputAction.CallbackContext context)
        {
            if (!context.started || !enabled) return;

            enabled = true;

            OnWeaponDrop?.Invoke();
            OnWeaponSwitched?.Invoke(WeaponType.Unarmed);
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (!context.started || OnAttack == null || !Health.IsAlive) return;

            enabled = false;

            OnAttack.Invoke();
        }

        public void Block(InputAction.CallbackContext context)
        {
            if (!Health.IsAlive) return;

            enabled = context.canceled;

            OnBlock?.Invoke(context);
        }

        public void Escape(InputAction.CallbackContext context)
        {
            if (!context.started || !Health.IsAlive) return;

            OnEscapePressed?.Invoke();
        }
    }
}