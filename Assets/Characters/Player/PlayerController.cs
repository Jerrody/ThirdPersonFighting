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

    public sealed class PlayerController : EntityController
    {
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

        private Vector3 _velocity = Vector3.zero;

        private bool _isJumpPressed;
        private bool _isJumping;
        private float _initialJumpVelocity;

        private void Start()
        {
            SetupGravityScale();
        }

        private void Update()
        {
            Controller.Move(_velocity * Time.deltaTime);
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
                _velocity = new Vector3(_velocity.x, groundedGravityForce, _velocity.z);
            }
            else
            {
                var previousYVelocity = _velocity.y;
                var newYVelocity = _velocity.y + (gravityForce * Time.deltaTime);
                var nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
                _velocity = new Vector3(_velocity.x, nextYVelocity, _velocity.z);
            }
        }

        private void HandleJump()
        {
            if (!_isJumping && Controller.isGrounded && _isJumpPressed)
            {
                _isJumping = true;
                _isJumpPressed = false;
                _velocity = new Vector3(_velocity.x, _initialJumpVelocity * 0.5f, _velocity.z);
            }
            else if (_isJumping && Controller.isGrounded && !_isJumpPressed)
            {
                _isJumping = false;
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            var axisValue = context.ReadValue<Vector2>();

            _velocity = new Vector3(walkSpeed * axisValue.x, _velocity.y, walkSpeed * axisValue.y);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started || context.canceled)
                _isJumpPressed = context.ReadValueAsButton();
        }
    }
}