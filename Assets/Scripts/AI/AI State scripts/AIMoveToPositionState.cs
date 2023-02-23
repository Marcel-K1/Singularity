/*****************************************************************************
* Project: Singularity
* File   : AIMoveToPositionState.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine state component 
* Main state for AI is to move around from waypoint to waypoint 
* or to found items, keys and gates.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveToPositionState : AIState
{

    [Header("Components")]
    [SerializeField]
    private NavMeshAgent navMeshAgent = new NavMeshAgent();

    [SerializeField]
    private int randomSearchAtAreaAttempts = 0;
    [SerializeField]
    private AIScriptableData aIScriptableData = null;

    [Header("Linked AI States")]
    [SerializeField]
    private AIFindRandomSearchPointState aIFindRandomSearchPointState = null;
    [SerializeField]
    private AISelectNewSearchPointState aISelectNewSearchPointState = null;
    [SerializeField]
    private AIFoundItemState aIFoundItemState = null;
    [SerializeField]
    private AIPickUPKeyState aIPickUPKeyState = null;
    [SerializeField]
    private AIUseGateState aIUseGateState = null;


    public override AIState RunCurrentAIState()
    {
        switch (aIScriptableData.AIState)
        {
            case AIScriptableData.EAIBehavior.AISelectNewSearchPointState:
                return aISelectNewSearchPointState;
            case AIScriptableData.EAIBehavior.AIFindRandomSearchPointState:
                return aIFindRandomSearchPointState;
            case AIScriptableData.EAIBehavior.AIFoundItemState:
                return aIFoundItemState;
            case AIScriptableData.EAIBehavior.AIMoveToKeyState:
                {
                    if (aIScriptableData.FoundItemCollider != null)
                    {
                        MoveToSearchPoint(aIScriptableData.FoundItemCollider.transform.position);
                        return this;
                    }
                    else
                    {
                        return aISelectNewSearchPointState;
                    }
                }
            case AIScriptableData.EAIBehavior.AIPickUPKeyState:
                return aIPickUPKeyState;
            case AIScriptableData.EAIBehavior.AIMoveToGatePositionState:
                {
                    if (aIScriptableData.FoundItemCollider != null)
                    {
                        MoveToSearchPoint(aIScriptableData.FoundItemCollider.transform.position);
                        return this;
                    }
                    else
                    {
                        return aISelectNewSearchPointState;
                    }
                }
            case AIScriptableData.EAIBehavior.AIUseGateState:
                return aIUseGateState;
            default:
                {
                    MoveToSearchPoint(aIScriptableData.RandomSearchPointAtSearchPoint);
                    return this;
                }
        }
    }

    /// <summary>
    /// Methode uses a searchpoint vector3 parameter to move the NPC and set up a random count for searches of the area at the searchpoint
    /// </summary>
    /// <param name="_searchPoint"></param>
    private void MoveToSearchPoint(Vector3 _searchPoint)
    {

        if (navMeshAgent == null)
            GetNavMashAgent();

        navMeshAgent.speed = aIScriptableData.AIRunSpeed;
        navMeshAgent.SetDestination(_searchPoint);
        Vector3 distanceToWalkPoint = transform.position - _searchPoint;

        // check for next step when AI moved close to the selected destination
        if (distanceToWalkPoint.magnitude < aIScriptableData.DistanceToWalkMagnitudeCheck)
        {
            if (randomSearchAtAreaAttempts > 0 && (aIScriptableData.AIState == AIScriptableData.EAIBehavior.AIMoveToPositionState))
            {
                aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIFindRandomSearchPointState;
                randomSearchAtAreaAttempts--;
            }
            else if (aIScriptableData.AIState == AIScriptableData.EAIBehavior.AIMoveToKeyState)
            {
                aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIPickUPKeyState;
            }
            else if(aIScriptableData.AIState == AIScriptableData.EAIBehavior.AIMoveToGatePositionState)
            {
                aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIUseGateState;
            }
            else
            {
                randomSearchAtAreaAttempts = UnityEngine.Random.Range(aIScriptableData.MinSearchAtSearchPointAttempts, aIScriptableData.MaxSearchAtSearchPointAttempts);
                aIScriptableData.AIState = AIScriptableData.EAIBehavior.AISelectNewSearchPointState;
            }
            

        }
    }
    
    private void GetNavMashAgent()      
    {          
        navMeshAgent = GetComponentInParent<NavMeshAgent>();      
    }
}
