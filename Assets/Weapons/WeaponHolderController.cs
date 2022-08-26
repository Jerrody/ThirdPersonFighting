using Characters.Animations;
using Characters.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public delegate bool WeaponOperations(MeleeWeaponController takeableWeapon);
    
    public sealed class WeaponHolderController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private PlayerController player;
        [SerializeField] private AnimationEventListener animEventListener;
        [SerializeField] private MeleeWeaponController[] weaponSlots = new MeleeWeaponController[2];

        public MeleeWeaponController CurrentActiveWeapon => weaponSlots[_activeSlotIndex];

        private WeaponType CurrentWeaponType =>
            CurrentActiveWeapon ? CurrentActiveWeapon.WeaponType : WeaponType.Unarmed;

        private int _activeSlotIndex;

        private void Awake()
        {
            player.OnWeaponDrop += OnWeaponDrop;
            player.OnAttack += OnAttack;
            player.OnHit += OnAttackEnd;
            player.OnWeaponSwitch += OnWeaponSwitch;

            animEventListener.OnAttackAnimationEnd += OnAttackEnd;
        }

        private void Start()
        {
            player.OnWeaponTake += OnWeaponTake;
            player.OnBlock.AddListener(OnBlock);
        }

        private WeaponType OnWeaponSwitch(int slotIndex)
        {
            if (_activeSlotIndex == slotIndex)
                return CurrentWeaponType;

            weaponSlots[_activeSlotIndex]?.gameObject.SetActive(false);

            _activeSlotIndex = slotIndex;

            var newActiveWeapon = weaponSlots[_activeSlotIndex];
            var newActiveWeaponType = CurrentWeaponType;

            if (newActiveWeaponType != WeaponType.Unarmed)
                newActiveWeapon.gameObject.SetActive(true);

            return newActiveWeaponType;
        }

        private void OnWeaponDrop()
        {
            if (weaponSlots[_activeSlotIndex] == null) return;

            weaponSlots[_activeSlotIndex].OnDrop();
            weaponSlots[_activeSlotIndex] = null;
        }

        private bool OnWeaponTake(MeleeWeaponController takenWeapon)
        {
            var result = false;
            for (var i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] != null) continue;

                if (CurrentActiveWeapon != null)
                {
                    CurrentActiveWeapon.gameObject.SetActive(false);

                    SetNewWeapon(takenWeapon, i);

                    OnWeaponSwitch(_activeSlotIndex);
                }
                else
                {
                    SetNewWeapon(takenWeapon, i);
                }

                takenWeapon.OnPickUp(transform);

                result = true;
                break;
            }

            return result;
        }

        private void SetNewWeapon(MeleeWeaponController newWeapon, int newWeaponIndex)
        {
            _activeSlotIndex = newWeaponIndex;
            weaponSlots[_activeSlotIndex] = newWeapon;
        }

        private void OnAttack()
        {
            if (weaponSlots[_activeSlotIndex] == null)
            {
                player.enabled = true;
            }
            else
            {
                weaponSlots[_activeSlotIndex]?.OnAttack();
            }
        }

        private void OnBlock(InputAction.CallbackContext context)
        {
            if (!context.performed)
                weaponSlots[_activeSlotIndex]?.OnBlock(context.started && !context.canceled);
        }

        private void OnAttackEnd()
        {
            weaponSlots[_activeSlotIndex]?.OnAttackEnd();
        }
    }
}