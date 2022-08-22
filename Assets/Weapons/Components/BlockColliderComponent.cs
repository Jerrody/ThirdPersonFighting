using Characters.Interfaces;
using UnityEngine;

namespace Weapons.Components
{
    public class BlockColliderComponent : MonoBehaviour, IDamageable
    {
        public void TakeDamage(uint damageAmount)
        {
        }

        public bool IsAlive()
        {
            return false;
        }
    }
}