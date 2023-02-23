/*****************************************************************************
* Project: Singularity
* File   : AIStateManager.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine manager component 
* Manager checks for collisions with items and manages the states.
* It also unstucks the AI if for some reason the state is not changed 
* wthin a specific interval.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{

    [SerializeField]
    AIState currentAIState = null;

    [SerializeField]
    private AIScriptableData aIScriptableData = null;

    [SerializeField, Range(1000, 3000)]
    private int maxStuckCounter = 1500;

    [SerializeField]
    private AIFindRandomSearchPointState aIDefaultState;

    [SerializeField]
    private int stuckCounter = 0;

    [SerializeField]
    private AIState lastAIState = null;


    private void Awake()
    {
        aIScriptableData.AIId = GetComponentInParent<Transform>();
        aIScriptableData.Coll = GetComponentInParent<SphereCollider>();
    }

    /// <summary>
    /// Trigger for found items.
    /// </summary>
    /// <param name="trigger"></param>
    private void OnTriggerEnter(Collider trigger)
    {

        if (aIScriptableData.AIState == AIScriptableData.EAIBehavior.AIMoveToPositionState)
        {
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIFoundItemState;
            aIScriptableData.FoundItemCollider = trigger;
        }
        
    }
    
    void Update()
    {
        
        RunAIStateMachine();

    }

    private void RunAIStateMachine()
    {
        AIState nextAIState = currentAIState?.RunCurrentAIState();

        if (nextAIState != null)
        {
            SwitchToNextAIState(nextAIState);
        }
    }

    private void SwitchToNextAIState(AIState nextAIState)
    {

        currentAIState = nextAIState;
        AIStuckCheckAndReset();

    }

    /// <summary>
    /// in case of the AI gets stuck in a state for around 30 seconds 
    /// (check is done once per frame) the function should reset the state
    /// </summary>
    /// <param name="nextAIState"></param>
    void AIStuckCheckAndReset()
    {
        if(lastAIState != null)
        {
            if(lastAIState == currentAIState)
            {
                stuckCounter++;
            }
            else
            {
                lastAIState = currentAIState;
                stuckCounter = 0;
            }
        }
        else
        {
            lastAIState = aIDefaultState;
        }

        if (stuckCounter > maxStuckCounter) 
        {
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISelectNewSearchPointState;
            currentAIState = aIDefaultState;
        }
    }
}
