using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public class WeaponHook : MonoBehaviour
    {
        public GameObject damageCollider;

        private void Start()
        {
            
        }
        public void DamageColliderStatus(bool status)
        {
            damageCollider.SetActive(status);
        }
    }
}