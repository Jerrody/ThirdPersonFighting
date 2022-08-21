using UnityEngine;
using Weapons;

namespace Characters.Player.Animations
{
    internal sealed class PlayerAnimationController : EntityAnimationController
    {
        [SerializeField] private PlayerController player;

        private static readonly int VelocityZParam = Animator.StringToHash("VelocityZ");
        private static readonly int VelocityXParam = Animator.StringToHash("VelocityX");

        private static WeaponType _previousWeaponType;

        private void Start()
        {
            player.onWeaponSwitched.AddListener(OnWeaponSwitched);
        }

        private void Update()
        {
            var velocity = player.Velocity;

            Anim.SetFloat(VelocityZParam, velocity.z, 0.1f, Time.deltaTime);
            Anim.SetFloat(VelocityXParam, velocity.x, 0.1f, Time.deltaTime);
        }

        private void OnWeaponSwitched(WeaponType currentWeaponType)
        {
            Anim.SetLayerWeight((int)_previousWeaponType, 0);
            Anim.SetLayerWeight((int)currentWeaponType, 1);

            _previousWeaponType = currentWeaponType;
        }
    }
}