using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class ItemActionContainer 
    {
        public string animName;
        public ItemAction itemAction;
        public AttackInputs attackInput;

        public void ExecuteItemAction(CharacterStateManager characterStateManager)
        {
            itemAction.ExecuteAction(characterStateManager);
        }
    }
}