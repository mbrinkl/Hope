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
        bool Rb, Rt, Lb, Lt, isAttacking;
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

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if (s.lockOn)
                {
                    s.OnClearLookOverride();
                }
                else
                {
                    s.OnAssignLookOverride(s.target);
                }
            }
            
            return retVal;
        }
        bool HandleAttacking()
        { 
            if (Rb || Rt || Lb || Lt)
            {
                isAttacking = true;
            }

            if (y_input)
            {
                isAttacking = false;
            }

            if (isAttacking)
            {
                //Find the actual attack animation from the items etc.
                //play animation
                s.PlayTargetAnimation("Attack1", true);
                s.ChangeState(s.attackStateId);
            }

            return isAttacking;
        }
    }
}
