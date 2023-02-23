/*****************************************************************************
* Project: Singularity
* File   : AIFindRandomSearchPointState.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine state component 
* State creates a new random search point for the AI.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindRandomSearchPointState : AIState
{

    [SerializeField]
    private AIScriptableData aIScriptableData = null;

    [Header("Layermasks for object identification")]
    [SerializeField]
    private LayerMask whatIsGround = -1;

    [Header("linked AI states")]
    [SerializeField]
    private AISelectNewSearchPointState aISelectNewSearchPointState = null;
    [SerializeField]
    private AIMoveToPositionState aIMoveToPositionState = null;


    public override AIState RunCurrentAIState()
    {

        switch (aIScriptableData.AIState)
        {
            case AIScriptableData.EAIBehavior.AIMoveToPositionState:
                return aIMoveToPositionState;

            default:
                {
                    RandomSearchPointForAreaSearchAtSearchPoint();
                    return this;
                }
        }


    }

    /// <summary>
    /// Creates a random searchpoint in the Area of the previousely selected searchpoint from the list
    /// </summary>
    private void RandomSearchPointForAreaSearchAtSearchPoint()
    {

        float randomZ = UnityEngine.Random.Range(-aIScriptableData.WalkPointRange, 
            aIScriptableData.WalkPointRange);

        float randomX = UnityEngine.Random.Range(-aIScriptableData.WalkPointRange, 
            aIScriptableData.WalkPointRange);

        float randomY = UnityEngine.Random.Range(-aIScriptableData.WalkPointRangeHeight, 
            aIScriptableData.WalkPointRangeHeight);

        aIScriptableData.RandomSearchPointAtSearchPoint = new Vector3(aIScriptableData.SearchPoint.x + randomX, 
            aIScriptableData.SearchPoint.y + randomY, aIScriptableData.SearchPoint.z + randomZ);

        if (Physics.Raycast(aIScriptableData.RandomSearchPointAtSearchPoint, -transform.up, 0.5f, whatIsGround))
            aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIMoveToPositionState;

    }

}
