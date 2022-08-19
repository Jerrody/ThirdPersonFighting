using Characters.Player;
using UnityEngine;

namespace Weapons
{
    public sealed class WeaponHolderController : MonoBehaviour
    {
        [SerializeField] private BasicWeaponController[] weaponSlots = new BasicWeaponController[2];
        [SerializeField] private PlayerController player;

        private int _activeSlotIndex;

        private void Awake()
        {
            player.OnWeaponTake.AddListener(OnWeaponTake);
            player.OnWeaponDrop.AddListener(OnWeaponDrop);
            player.OnWeaponSwitch.AddListener(OnWeaponSwitch);
        }

        private void OnWeaponSwitch(int slotIndex)
        {
            _activeSlotIndex = slotIndex;

            weaponSlots[_activeSlotIndex == 0 ? 1 : 0].gameObject.SetActive(false);
            weaponSlots[_activeSlotIndex].gameObject.SetActive(true);
        }

        private void OnWeaponDrop()
        {
            Destroy(weaponSlots[_activeSlotIndex].gameObject);
            weaponSlots[_activeSlotIndex] = null;
        }

        private void OnWeaponTake(BasicWeaponController takenWeapon)
        {
            for (var i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i] != null) return;

                var weapon = Instantiate(takenWeapon, takenWeapon.GetPositionInHands(),
                    takenWeapon.GetRotationInHands());
                weaponSlots[i] = weapon;

                Destroy(takenWeapon.gameObject);
            }
        }

        public WeaponType GetCurrentWeaponType()
        {
            return weaponSlots[_activeSlotIndex].WeaponType;
        }
    }
}