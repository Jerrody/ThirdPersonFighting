using Characters.Player;
using UnityEngine;

namespace Weapons
{
    public sealed class WeaponHolderController : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private MeleeWeaponController[] weaponSlots = new MeleeWeaponController[2];

        private WeaponType CurrentWeaponType { get; set; }
        private int _activeSlotIndex;

        private void Start()
        {
            player.onWeaponTake.AddListener(OnWeaponTake);
            player.onWeaponDrop.AddListener(OnWeaponDrop);
            player.OnWeaponSwitch += OnWeaponSwitch;
        }

        private WeaponType OnWeaponSwitch(int slotIndex)
        {
            if (_activeSlotIndex == slotIndex) return CurrentWeaponType;
            weaponSlots[_activeSlotIndex]?.gameObject.SetActive(false);

            _activeSlotIndex = slotIndex;

            var newActiveWeapon = weaponSlots[_activeSlotIndex];
            if (newActiveWeapon == null) return WeaponType.Unarmed;

            newActiveWeapon.gameObject.SetActive(true);
            CurrentWeaponType = newActiveWeapon.GetWeaponType();

            return CurrentWeaponType;
        }

        private void OnWeaponDrop()
        {
            weaponSlots[_activeSlotIndex].OnDrop();
            weaponSlots[_activeSlotIndex] = null;
        }

        private void OnWeaponTake(MeleeWeaponController takenWeapon)
        {
            for (var i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] != null) continue;

                weaponSlots[i] = takenWeapon;
                _activeSlotIndex = i;
                takenWeapon.OnPickUp(transform);

                break;
            }
        }
    }
}