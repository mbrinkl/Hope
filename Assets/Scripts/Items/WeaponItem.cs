using UnityEngine;
using System.Collections;

namespace FSM
{
    [CreateAssetMenu(menuName ="FSM/Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public ItemActionContainer[] itemActions;

        [System.NonSerialized]
        public WeaponHook weaponHook;
        public void HandleDamageCollider(bool status)
        {
            if (status == false)
            { 
                
            }
            else 
            {
                
            }
        }
    }
}