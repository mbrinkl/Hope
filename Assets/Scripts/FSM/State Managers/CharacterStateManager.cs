using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FSM
{
    public abstract class CharacterStateManager : StateManager
    {
        [Header("References")]
        public Animator anim;
        public new Rigidbody rigidbody;
        public AnimatorHook animHook;
        public SphereCollider sphereCollider;

        [Header("States")]
        public bool isGrounded;
        public bool useRootMotion;
        public bool lockOn;
        public Transform target;

        [Header("Controller Values")]
        public float vertical;
        public float horizontal;
        public float delta;
        public Vector3 rootMovement;
        public string[] animNames = { "Attack1", "Attack2", "Attack3", "Attack4", "Attack5" };


        [Header("Item actions")]
        ItemActionContainer[] itemActions = new ItemActionContainer[1];
        public ItemActionContainer[] defaultItemActions = new ItemActionContainer[4];
        [Header("Runtime References")]
        public WeaponItem weapon;
        public GameObject model;
 

        public override void Init()
        {
            anim = GetComponentInChildren<Animator>();
            animHook = GetComponentInChildren<AnimatorHook>();
            rigidbody = GetComponentInChildren<Rigidbody>();
            anim.applyRootMotion = true;
            target = FindClosestEnemy();
            animHook.Init(this);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetItemAction(AttackInputs attackInput)
        { 
        switch(attackInput)
            {
                case AttackInputs.Rb:
                    itemActions[0].ExecuteItemAction(this);
                    break;
                case AttackInputs.Rt:
                    itemActions[1].ExecuteItemAction(this);
                    break;
                case AttackInputs.Lb:
                    itemActions[2].ExecuteItemAction(this);
                    break;
                case AttackInputs.Lt:
                    itemActions[3].ExecuteItemAction(this);
                    break;
                default:
                    break;
            }
        }

        public virtual void OnAssignLookOverride(Transform target)
        {
            this.target = target;
            if (target != null)
                lockOn = true;
            if (target == null)
                OnClearLookOverride();
        }

        public virtual void OnClearLookOverride()
        {
            lockOn = false;
        }

        public void assignCurrentWeapon(WeaponItem weapon)
        {
            this.weapon = weapon;
        }

        public void HandleDamageCollider(bool status)
        {  
            weapon.weaponHook.DamageColliderStatus(status);      
        }

        public Transform FindClosestEnemy()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Enemy");

            if (gos.Length == 0)
                return null;

            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            for(int i = 0; i < gos.Length; i++)
            {
                Vector3 diff = gos[i].transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = gos[i];
                    distance = curDistance;
                }
            }
            return closest.transform;
        }
        public void Update()
        {

            if (target == null)
            {
                lockOn = false;
            }

        }
    }
}
