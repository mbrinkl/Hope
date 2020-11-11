using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class WeaponHolderHook : MonoBehaviour
    {
        public Transform parentOverride;

        public GameObject currentModel;
  
        public void showWeapon()
        {
            currentModel.transform.localScale = new Vector3(1, 1, 1);
        }

        public void disableWeapon()
        {
            currentModel.transform.localScale = new Vector3(0, 0, 0);
        }
    }
       
}