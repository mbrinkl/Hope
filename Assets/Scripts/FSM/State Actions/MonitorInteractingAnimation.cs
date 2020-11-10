using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class MonitorInteractingAnimation : StateAction
    {
        CharacterStateManager states;
        string targetBool;
        string targetState;

        public MonitorInteractingAnimation(CharacterStateManager characterStateManager, string targetBool, string targetState)
        {
            states = characterStateManager;
            this.targetBool = targetBool;
            this.targetState = targetState;
        }

        public override bool Execute()
        {
            bool isInteracting = states.anim.GetBool("isInteracting");

            if (isInteracting)
            {
                return false;
            }
            else
            {
                states.ChangeState(targetState);
                return true;
            }
        }
    }
}