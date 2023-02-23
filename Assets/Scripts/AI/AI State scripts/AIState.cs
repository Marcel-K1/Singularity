/*****************************************************************************
* Project: Singularity
* File   : AIState.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* AI State Machnine abstract class.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public abstract AIState RunCurrentAIState();
}
