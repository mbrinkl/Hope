using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class InputManager : StateAction
    {
        PlayerStateManager s;
        //triggers and bumpers
        bool Rb, Rt, Lb, Lt, isAttacking, isEquipped = false;
        //inventory
        bool inventoryInput;
        //prompts
        bool b_input, y_input, x_input;
        //dpad
        bool leftArrow, rightArrow, upArrow, downArrow;

        public InputManager(PlayerStateManager states)
        {
            s = states;    
        }

        public override bool Execute()
        {
            bool retVal = false;
            
            isAttacking = false;

            s.horizontal = Input.GetAxis("Horizontal");
            s.vertical = Input.GetAxis("Vertical");
            Rb = Input.GetButton("RB");
            Rt = Input.GetButton("RT");
            Lb = Input.GetButton("LB");
            Lt = Input.GetButton("LT");

            inventoryInput = Input.GetButton("Inventory");
            b_input = Input.GetButton("B");
            y_input = Input.GetButton("Y");
            x_input = Input.GetButton("X");
            leftArrow = Input.GetButton("Left");
            rightArrow = Input.GetButton("Right");
            upArrow = Input.GetButton("Up");
            downArrow = Input.GetButton("Down");
            s.mouseX = Input.GetAxis("Mouse X");
            s.mouseY = Input.GetAxis("Mouse Y");

            s.moveAmount = Mathf.Clamp01(Mathf.Abs(s.horizontal) + Mathf.Abs(s.vertical));

            retVal = HandleAttacking();
            if (!isEquipped)
            {
                s.OnClearLookOverride();
            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (s.lockOn)
                {
                    s.OnClearLookOverride();
                }
                else
                {
                    if(isEquipped)
                    s.OnAssignLookOverride(s.target);
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                HandleisEquipped();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                HandleDodge();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && isEquipped)
            {
                s.anim.SetBool("isBlocking", true);
                s.movementSpeed = 0;
                HandleBlock();
            }

            if (Input.GetKeyUp(KeyCode.Mouse1) && isEquipped)
            {
                s.anim.SetBool("isBlocking", false);
                s.movementSpeed = 8;
            }

            if (Input.GetKeyDown(KeyCode.Q) && isEquipped)
            {
                HandleSpecialAttack();
            }

            if (isAttacking)
            {
                //s.movementSpeed = 0;
            }
            return retVal;
        }
        
        bool HandleAttacking()
        {
            

            if (Rb)
            {
                isAttacking = true;
                AttackInputs attackInput = AttackInputs.Rb;

                if (Rb)
                {
                    attackInput = AttackInputs.Rb;
                }

                if (Rt)
                {
                    attackInput = AttackInputs.Rt;
                }

                if (Lb)
                {
                    attackInput = AttackInputs.Lb;
                }

                if (Lt)
                {
                    attackInput = AttackInputs.Lt;
                }
            }

            if (y_input)
            {
                if (isAttacking)
                {

                }
                else
                {
                    isAttacking = false;
                }
            }

            if (isAttacking && isEquipped)
            {

                //Find the actual attack animation from the items etc.
                //play animation
                if (s.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .65) 
                {
                        //s.movementSpeed = 0;
                    s.anim.SetTrigger("Attack");
                }

                s.ChangeState(s.attackStateId);
            }
            return isAttacking;
        }

        void HandleDodge()
        {
            if (isEquipped)
            {
                s.anim.SetTrigger("Dodge");

                //s.PlayTargetAnimation("Dodge",true);
                s.ChangeState(s.attackStateId);
            }
        }

        void HandleBlock()
        {
            if (isEquipped)
            {
                s.anim.SetTrigger("Block");
                s.ChangeState(s.attackStateId);
            }
        }

        void HandleisEquipped()
        {
                if (isEquipped == false)
                {
                    s.anim.SetBool("isEquipped", true);
                    s.PlayTargetAnimation("Equip", true);
                    s.ChangeState(s.attackStateId);
                    isEquipped = true;
                }
                else
                {
                    s.anim.SetBool("isEquipped", false);
                    s.PlayTargetAnimation("Unequip", true);
                    s.ChangeState(s.attackStateId);
                    isEquipped = false;
                }   
            }
        void HandleSpecialAttack()
        {
            s.PlayTargetAnimation("Special Start", true);
            s.ChangeState(s.attackStateId);
        }
    }
}

