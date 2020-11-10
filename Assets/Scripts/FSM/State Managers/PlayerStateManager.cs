using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class PlayerStateManager : CharacterStateManager
    {
        [Header("Inputs")]
        public float mouseX;
        public float mouseY;
        public float moveAmount;
        public Vector3 rotateDirection;
        
        [Header("References")]
        public new Transform camera;
        public Cinemachine.CinemachineFreeLook mainCamera;
        public Cinemachine.CinemachineFreeLook lockOnCamera;

        [Header("Movement Stats")]
        public float frontRayOffset = .5f;
        public float movementSpeed = 2;
        public float adaptSpeed = 10;
        public float rotationSpeed = 10;

        [HideInInspector]
        public LayerMask ignoreForGroundCheck;

        [HideInInspector]
        public string locomotionId = "locomotion";
        [HideInInspector]
        public string attackStateId = "attackState";
        public override void Init()
        {
            base.Init();

            State locomotion = new State(
                new List<StateAction>() //fixed update
                {
                    new MovePlayerCharacter(this)
                },
                new List<StateAction>() //update
                {
                    new InputManager(this),
                },
                new List<StateAction>() //late update
                {
                }
                );
            locomotion.onEnter = DisableRootMotion;

            State attackState = new State(
                new List<StateAction>() //fixed update
                {
                },
                new List<StateAction>() //update
                {
                    new MonitorInteractingAnimation(this, "isInteracting", locomotionId),
                },
                new List<StateAction>() //late update
                {
                }
                );

            attackState.onEnter = EnableRootMotion;

            Registerstate(locomotionId, locomotion);
            Registerstate(attackStateId, attackState);

            ChangeState(locomotionId);

            ignoreForGroundCheck = ~(1 << 9 | 1 << 10);

        }

        private void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            FixedTick();
        }

        private bool debugLock;

        private void Update()
        {
            delta = Time.deltaTime;
            LateTick();
        }

        private void LateUpdate()
        {
            Tick();
        }



        #region State Events
        void DisableRootMotion()
        {
            useRootMotion = false;
        }

        void EnableRootMotion()
        {
            useRootMotion = true;
        }
        #endregion

        #region Lock On
        public override void OnAssignLookOverride(Transform target)
        {
            base.OnAssignLookOverride(target);
            if (lockOn == false)
            {
                return;
            }

            mainCamera.gameObject.SetActive(false);
            lockOnCamera.gameObject.SetActive(true);
            lockOnCamera.m_LookAt = target;
        }

        public override void OnClearLookOverride()
        {
            base.OnClearLookOverride();
            mainCamera.gameObject.SetActive(true);
            lockOnCamera.gameObject.SetActive(false);
        }
        #endregion

    }
}