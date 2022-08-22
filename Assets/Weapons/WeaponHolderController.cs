using Characters.Player;
using Characters.Player.Animations;
using UnityEngine;

namespace Weapons
{
    public sealed class WeaponHolderController : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private AnimationEventListener animEventListener;
        [SerializeField] private MeleeWeaponController[] weaponSlots = new MeleeWeaponController[2];

        public MeleeWeaponController CurrentActiveWeapon => weaponSlots[_activeSlotIndex];
        private WeaponType CurrentWeaponType { get; set; }
        private int _activeSlotIndex;

        private bool _blockStarted;
        private bool _blockHolding;
        private bool _blockEnded;

        private void Start()
        {
            player.onWeaponTake.AddListener(OnWeaponTake);
            player.onWeaponDrop.AddListener(OnWeaponDrop);
            player.OnWeaponSwitch += OnWeaponSwitch;
        }

        private WeaponType OnWeaponSwitch(int slotIndex)
        {
            if (_activeSlotIndex == slotIndex)
                return CurrentActiveWeapon == null ? WeaponType.Unarmed : CurrentWeaponType;

            weaponSlots[_activeSlotIndex]?.gameObject.SetActive(false);

            _activeSlotIndex = slotIndex;

            var newActiveWeapon = weaponSlots[_activeSlotIndex];
            if (newActiveWeapon == null)
            {
                CurrentWeaponType = WeaponType.Unarmed;
                return CurrentWeaponType;
            }

            newActiveWeapon.gameObject.SetActive(true);
            CurrentWeaponType = newActiveWeapon.GetWeaponType();

            return CurrentWeaponType;
        }

        private void OnWeaponDrop()
        {
            if (weaponSlots[_activeSlotIndex] == null) return;

            weaponSlots[_activeSlotIndex].OnDrop();
            weaponSlots[_activeSlotIndex] = null;
        }

        private void OnWeaponTake(MeleeWeaponController takenWeapon)
        {
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

                player.OnAttack += OnAttack;
                animEventListener.OnAttackAnimationEnd += OnAttackEnd;
                takenWeapon.OnPickUp(transform);
                CurrentWeaponType = CurrentActiveWeapon.GetWeaponType();

                break;
            }
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

        private void OnBlock()
        {
            weaponSlots[_activeSlotIndex]?.OnBlock();
        }

        public void OnAttackEnd()
        {
            weaponSlots[_activeSlotIndex].OnAttackEnd();
        }
    }
}