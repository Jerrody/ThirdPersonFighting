using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public delegate void Notify();

    public enum CurrentWeapon
    {
        Unarmed = 1,
        Sword = 2,
        Axe = 3,
        Bow = 4,
    }

    // TODO: Remove jump mechanics.
    public sealed class PlayerController : EntityController
    {
        // TODO: Make weapon switching and notifying about that.
        public event Notify WeaponSwitched;

        [SerializeField, Range(float.MinValue, 0.0f)]
        private float gravityForce = -9.8f;

        [SerializeField, Range(float.MinValue, 0.0f)]
        private float groundedGravityForce = -0.5f;

        [SerializeField, Range(0, float.MaxValue)]
        private float maxJumpHeight = 100.0f;

        [SerializeField, Range(0, float.MaxValue)]
        private float maxJumpTime = 0.5f;

        public CurrentWeapon CurrentWeapon { get; private set; } = CurrentWeapon.Unarmed;
        public Vector3 Velocity { get; private set; }

        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _inputMoveAxis = Vector2.zero;

        private bool _isJumpPressed;
        private bool _isJumping;
        private float _initialJumpVelocity;

        private void Start()
        {
            SetupGravityScale();
        }

        private void Update()
        {
            _moveDirection = new Vector3(_inputMoveAxis.x, _moveDirection.y, _inputMoveAxis.y);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= walkSpeed;

            Controller.Move(_moveDirection * Time.deltaTime);

            HandleGravity();
            HandleJump();
        }

        private void SetupGravityScale()
        {
            var timeToApexTime = maxJumpTime / 2.0f;
            gravityForce = (-2.0f * maxJumpHeight) / Mathf.Pow(timeToApexTime, 2.0f);
            _initialJumpVelocity = (2.0f * maxJumpHeight) / timeToApexTime;
        }

        private void HandleGravity()
        {
            if (Controller.isGrounded)
            {
                _moveDirection = new Vector3(_moveDirection.x, groundedGravityForce, _moveDirection.z);
            }
            else
            {
                var previousYVelocity = _moveDirection.y;
                var newYVelocity = _moveDirection.y + (gravityForce * Time.deltaTime);
                var nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
                _moveDirection = new Vector3(_moveDirection.x, nextYVelocity, _moveDirection.z);
            }
        }

        private void HandleJump()
        {
            if (!_isJumping && Controller.isGrounded && _isJumpPressed)
            {
                _isJumping = true;
                _isJumpPressed = false;
                _moveDirection = new Vector3(_moveDirection.x, _initialJumpVelocity * 0.5f, _moveDirection.z);
            }
            else if (_isJumping && Controller.isGrounded && !_isJumpPressed)
            {
                _isJumping = false;
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            _inputMoveAxis = context.ReadValue<Vector2>();

            Velocity = new Vector3(_inputMoveAxis.x * walkSpeed, _moveDirection.y, _inputMoveAxis.y * walkSpeed);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started || context.canceled)
                _isJumpPressed = context.ReadValueAsButton();
        }
    }
}