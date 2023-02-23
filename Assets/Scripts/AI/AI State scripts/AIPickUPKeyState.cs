/*****************************************************************************
* Project: Singularity
* File   : AIPickUPKeyState.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine state component 
* When an found item is idntified as a key, the AI should move to the key
* and pick it up.
* In case of enough collected keys, the AI should move to a found gate 
* or search for the gate.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPickUPKeyState : AIState
{

    [SerializeField]
    private AIScriptableData aIScriptableData = null;

    [Header("Linked AI States")]
    [SerializeField]
    private AIMoveToPositionState aIMoveToPositionState = null;
    [SerializeField]
    private AISelectGatePositionState aISelectGatePositionState = null;

    public override AIState RunCurrentAIState()
    {

        switch (aIScriptableData.AIState)
        {
            
            case AIScriptableData.EAIBehavior.AIPickUPKeyState:
                {
                    PickUpKey();
                    return this;
                }
            case AIScriptableData.EAIBehavior.AIMoveToPositionState:
                {
                    return aIMoveToPositionState;
                }
            case AIScriptableData.EAIBehavior.AISelectGatePositionState:
                {
                    return aISelectGatePositionState;
                }
            default:
                return this;
            
        }
    
    }

    /// <summary>
    /// function to picup found keys
    /// </summary>
    public void PickUpKey()
    {
        if (aIScriptableData.FoundItemCollider != null)
        {

            if (aIScriptableData.FoundItemCollider.gameObject.tag == "Key")
            {
                aIScriptableData.FoundKeys++;

                Destroy(aIScriptableData.FoundItemCollider.gameObject);

                aIScriptableData.FoundItemCollider = null;

                if (aIScriptableData.FoundKeys < aIScriptableData.RequiredKeysToUnlockGate)
                {
                    aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
                }
                else
                {
                    aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISelectGatePositionState;
                }
            }
            else if (aIScriptableData.FoundKeys >= aIScriptableData.RequiredKeysToUnlockGate)
            {
                // security check to ensure that no strange issues happen 
                aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISelectGatePositionState;
            }
            else
            {
                aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
            }

        }
        else
        {
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
            aIScriptableData.FoundItemCollider = null;
        }
    }
}
