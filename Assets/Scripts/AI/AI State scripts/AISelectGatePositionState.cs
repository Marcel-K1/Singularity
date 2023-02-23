/*****************************************************************************
* Project: Singularity
* File   : AIFoundItemState.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine state component 
* Identifies a found game object (gate, key, item) 
* and decides what to do with this found object.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISelectGatePositionState : AIState
{
    [SerializeField]
    private AIScriptableData aIScriptableData = null;

    [Header("Linked AI States")]
    [SerializeField]
    private AIMoveToPositionState aIMoveToGatePositionState = null;


    public override AIState RunCurrentAIState()
    {
        switch (aIScriptableData.AIState)
        {
            case AIScriptableData.EAIBehavior.AISelectGatePositionState:
                {
                    SelectGateToMove();
                    return this;
                }
            case AIScriptableData.EAIBehavior.AIMoveToGatePositionState:
                {
                    return aIMoveToGatePositionState;
                }
            case AIScriptableData.EAIBehavior.AISearchForGateState:
                {
                    return aIMoveToGatePositionState;
                }
            default:
                {
                    return this;
                }
        }
    }

    /// <summary>
    /// selects next game form AI memory when required keys to open a gate are collected
    /// </summary>
    void SelectGateToMove()
    {
        if ((aIScriptableData.FoundGateLocation != null) && (aIScriptableData.FoundGateLocation.Count != 0))
        {
            aIScriptableData.FoundItemCollider = aIScriptableData.FoundGateLocation.Pop();
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToGatePositionState;
        }
        else
        {
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISearchForGateState;

        }
    }
}
