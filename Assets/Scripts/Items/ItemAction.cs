using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class ItemAction : ScriptableObject
    {
        public abstract void ExecuteAction(CharacterStateManager characterStateManager);
    }
}