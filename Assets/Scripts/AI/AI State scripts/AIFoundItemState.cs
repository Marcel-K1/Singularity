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

public class AIFoundItemState : AIState
{

    [SerializeField]
    private AIScriptableData aIScriptableData = null;

    [Header("Linked AI States")]
    [SerializeField]
    private AIMoveToPositionState aIMoveToPositionState = null;
    [SerializeField]
    private AIMoveToPositionState aIMoveToKeyState = null;

    public override AIState RunCurrentAIState()
    {
        switch (aIScriptableData.AIState)
        {
            case AIScriptableData.EAIBehavior.AIFoundItemState:
                {
                    IdentifyItem();
                    return this;
                }
            case AIScriptableData.EAIBehavior.AIMoveToPositionState:
                return aIMoveToPositionState;
            case AIScriptableData.EAIBehavior.AIMoveToKeyState:
                return aIMoveToKeyState;
            default:
                {
                    Debug.LogError("There may be something wrong in logic!");
                    return aIMoveToPositionState;
                }
        }
    }
 
    /// <summary>
    /// function to identify found items 
    /// </summary>
    private void IdentifyItem()
    {

        if (aIScriptableData.FoundItemCollider != null)
        {
            switch (aIScriptableData.FoundItemCollider.gameObject.tag)
            {
                case "Gate":
                    {

                        // adds game position to AI memory in case it is not known already
                        if (!aIScriptableData.FoundGateLocation.Contains(aIScriptableData.FoundItemCollider))
                            aIScriptableData.FoundGateLocation.Push(aIScriptableData.FoundItemCollider);

                        // checks if the AI has already enough keys collected to open a gate
                        if (aIScriptableData.FoundKeys < aIScriptableData.RequiredKeysToUnlockGate)
                        {
                            aIScriptableData.FoundItemCollider = null;
                            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
                        }
                        else
                            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToKeyState;

                        break;
                    }
                case "Key":
                    {
                        // collects a found key if the AI does not have the required number of keys collected
                        if (aIScriptableData.FoundKeys < aIScriptableData.RequiredKeysToUnlockGate)
                            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToKeyState;
                        else
                        {
                            aIScriptableData.FoundItemCollider = null;
                            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
                        }

                        break;
                    }
                case "Item":
                    {
                        // currently unused!
                        aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
                        aIScriptableData.FoundItemCollider = null;
                        break;
                    }
                default:
                    {
                        aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
                        aIScriptableData.FoundItemCollider = null;
                        break;
                    }
            }
        }
        else 
        {
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;
            aIScriptableData.FoundItemCollider = null;
        }
    }
}
