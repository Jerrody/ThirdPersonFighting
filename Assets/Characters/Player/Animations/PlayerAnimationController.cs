using UnityEngine;
using Weapons;

namespace Characters.Player.Animations
{
    internal sealed class PlayerAnimationController : EntityAnimationController
    {
        [Header("References")]
        [SerializeField] private PlayerController player;
        [SerializeField] private WeaponHolderController weaponHolder;

        private static readonly int VelocityZParam = Animator.StringToHash("VelocityZ");
        private static readonly int VelocityXParam = Animator.StringToHash("VelocityX");
        private static readonly int AttackIndexParam = Animator.StringToHash("AttackIndex");
        private static readonly int BlockParam = Animator.StringToHash("Block");

        private WeaponType _previousWeaponType = WeaponType.Unarmed;

        private void Start()
        {
            player.OnWeaponSwitched.AddListener(OnWeaponSwitched);
            player.OnHit += OnHit;
            player.OnDeath += OnDeath;
        }

        private void Update()
        {
            var velocity = player.Velocity;

            Anim.SetFloat(VelocityZParam, velocity.z, 0.1f, Time.deltaTime);
            Anim.SetFloat(VelocityXParam, velocity.x, 0.1f, Time.deltaTime);
        }

        private void OnWeaponSwitched(WeaponType newWeaponType)
        {
            if (weaponHolder.CurrentActiveWeapon != null)
            {
                weaponHolder.CurrentActiveWeapon.OnAttackAnimation += OnAttack;
                weaponHolder.CurrentActiveWeapon.OnBlockAnimation += OnBlock;
            }

            Anim.SetLayerWeight((int)_previousWeaponType, 0);
            _previousWeaponType = newWeaponType;

            Anim.SetLayerWeight((int)newWeaponType, 1);
        }

        private void OnAttack(uint attackIndex)
        {
            Anim.SetTrigger(AttackParam);
            Anim.SetInteger(AttackIndexParam, (int)attackIndex);
        }

        private void OnBlock(bool isBlocking)
        {
            Anim.SetBool(BlockParam, isBlocking);
        }

        private void OnHit()
        {
            Anim.SetTrigger(HitParam);
        }

        private void OnDeath()
        {
            player.OnDeath -= OnDeath;

            Anim.SetBool(DeadParam, true);
        }
    }
}