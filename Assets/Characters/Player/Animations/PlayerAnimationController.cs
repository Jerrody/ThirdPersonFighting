using UnityEngine;

namespace Characters.Player
{
    internal sealed class PlayerAnimationController : EntityAnimationController
    {
        [SerializeField] private PlayerController player;

        private static readonly int VelocityZParam = Animator.StringToHash("VelocityZ");
        private static readonly int VelocityXParam = Animator.StringToHash("VelocityX");

        private void Start()
        {
            player.WeaponSwitched += WeaponSwitched;
        }

        private void Update()
        {
            var velocity = player.Velocity;

            Anim.SetFloat(VelocityZParam, velocity.z, 0.1f, Time.deltaTime);
            Anim.SetFloat(VelocityXParam, velocity.x, 0.1f, Time.deltaTime);
        }

        private void WeaponSwitched()
        {
            Anim.SetLayerWeight((int)player.CurrentWeapon - 1, 0);
            Anim.SetLayerWeight((int)player.CurrentWeapon, 1);
        }
    }
}