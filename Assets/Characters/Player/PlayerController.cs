using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerController : EntityController
    {
        [SerializeField, Range(float.MinValue, 0.0f)]
        private float gravityForce = -9.8f;

        [SerializeField, Range(float.MinValue, 0.0f)]
        private float groundedGravityForce = -0.5f;

        [SerializeField, Range(0, float.MaxValue)]
        private float maxJumpHeight = 100.0f;

        [SerializeField, Range(0, float.MaxValue)]
        private float maxJumpTime = 0.5f;

        public Vector3 Velocity { get; private set; } = Vector3.zero;

        private bool _isJumpPressed;
        private bool _isJumping;
        private float _initialJumpVelocity;

        private void Start()
        {
            SetupGravityScale();
        }

        private void Update()
        {
            Controller.Move(Velocity * Time.deltaTime);
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
                Velocity = new Vector3(Velocity.x, groundedGravityForce, Velocity.z);
            }
            else
            {
                var previousYVelocity = Velocity.y;
                var newYVelocity = Velocity.y + (gravityForce * Time.deltaTime);
                var nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
                Velocity = new Vector3(Velocity.x, nextYVelocity, Velocity.z);
            }
        }

        private void HandleJump()
        {
            if (!_isJumping && Controller.isGrounded && _isJumpPressed)
            {
                _isJumping = true;
                _isJumpPressed = false;
                Velocity = new Vector3(Velocity.x, _initialJumpVelocity * 0.5f, Velocity.z);
            }
            else if (_isJumping && Controller.isGrounded && !_isJumpPressed)
            {
                _isJumping = false;
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            var axisValue = context.ReadValue<Vector2>();

            Velocity = new Vector3(walkSpeed * axisValue.x, Velocity.y, walkSpeed * axisValue.y);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started || context.canceled)
                _isJumpPressed = context.ReadValueAsButton();
        }
    }
}