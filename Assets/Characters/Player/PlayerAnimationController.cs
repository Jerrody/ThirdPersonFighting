using UnityEngine;

namespace Characters.Player
{
    internal sealed class PlayerAnimationController : EntityAnimationController
    {
        [SerializeField] private PlayerController player;

        private static readonly int WalkingForward = Animator.StringToHash("WalkingForward");
        private static readonly int WalkingBackward = Animator.StringToHash("WalkingBackward");
        private static readonly int WalkingRight = Animator.StringToHash("WalkingRight");
        private static readonly int WalkingLeft = Animator.StringToHash("WalkingLeft");

        private bool _isWalkingForward;
        private bool _isWalkingBackward;
        private bool _isWalkingRight;
        private bool _isWalkingLeft;

        private void Update()
        {
            var velocity = player.Velocity;
            _isWalkingBackward = velocity.z < 0.0f && velocity.x == 0.0f;
            _isWalkingForward = velocity.z > 0.0f && velocity.x == 0.0f;
            _isWalkingRight = velocity.z == 0.0f && velocity.x > 0.0f;
            _isWalkingLeft = velocity.z == 0.0f && velocity.x < 0.0f;

            Anim.SetBool(WalkingForward, _isWalkingForward);
            Anim.SetBool(WalkingBackward, _isWalkingBackward);
            Anim.SetBool(WalkingRight, _isWalkingRight);
            Anim.SetBool(WalkingLeft, _isWalkingLeft);
        }
    }
}