/*****************************************************************************
* Project: Singularity
* File   : AIUseGateState.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine state component 
* State manages the use Gate behavior for the AI.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUseGateState : AIState
{

    [SerializeField]
    private AIScriptableData aIScriptableData = null;
    
    [Header("Linked AI States")]
    [SerializeField]
    private AISelectGatePositionState aISelectGatePositionState = null;
    [SerializeField]
    private AIMoveToPositionState aIMoveToPositionState = null;


    public override AIState RunCurrentAIState()
    {
        switch (aIScriptableData.AIState)
        {
            case AIScriptableData.EAIBehavior.AIUseGateState:
                {
                    UseGate();
                    return this;
                }
            case AIScriptableData.EAIBehavior.AISelectGatePositionState:
                {
                    return aISelectGatePositionState;
                }
            default:
                {                
                    return aIMoveToPositionState;
                }
        }
    }

    void UseGate()
    {
        if (aIScriptableData.FoundItemCollider != null)
        {
            switch (aIScriptableData.FoundItemCollider.gameObject.tag)
            {
                case "Gate":
                    {               

                        aIScriptableData.FoundItemCollider.gameObject.GetComponent<GateController>().Tag = "UsedGate";
                        LevelManager.Instance.CheckForAI();
                        aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISelectNewSearchPointState;
                        aIScriptableData.AIId.gameObject.SetActive(false);
                        break;
                    }
                default:
                    {
                        aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISelectGatePositionState;
                        aIScriptableData.FoundItemCollider = null;
                        break;
                    }
            }
        }
        else
        {
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISelectGatePositionState;
            aIScriptableData.FoundItemCollider = null;
        }

    }
}
