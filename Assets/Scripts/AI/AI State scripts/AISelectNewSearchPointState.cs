/*****************************************************************************
* Project: Singularity
* File   : AISelectNewSearchPointState.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine state component 
* State selects a new search point location for AI search behavior from a list
* of possible search points.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISelectNewSearchPointState : AIState
{

    [SerializeField]
    private AIScriptableData aIScriptableData = null;

    [SerializeField]
    public List<Transform> possibleSearchPoint = new List<Transform>();

    [Header("Linked AI States")]
    [SerializeField]
    private AIFindRandomSearchPointState aIFindRandomSearchPointState = null;


    public override AIState RunCurrentAIState()
    {

        switch (aIScriptableData.AIState)
        {
            case AIScriptableData.EAIBehavior.AIFindRandomSearchPointState:
                return aIFindRandomSearchPointState;               
            default:
                {
                    SelectNewSearchPoint();
                    return this;
                }
        }  


    }

    /// <summary>
    /// Selects a random searchpoint from the searchpoint List
    /// </summary>
    private void SelectNewSearchPoint()
    {

        if ((possibleSearchPoint == null) || (possibleSearchPoint.Count == 0))
        {
            FillSearchPoint();
        }

        aIScriptableData.SearchPoint = possibleSearchPoint[Random.Range(0, 
            possibleSearchPoint.Count)].position;

        aIScriptableData.AIState = AIScriptableData.EAIBehavior.AIFindRandomSearchPointState;

    }

    private void FillSearchPoint()
    {
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("SearchPoint"))
        {
            possibleSearchPoint.Add(GO.transform);

        };
    }
}
