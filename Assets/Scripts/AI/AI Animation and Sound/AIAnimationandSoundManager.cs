/*****************************************************************************
* Project: Singularity
* File   : AIAnimationandSoundManager.cs
* Date   : 16.02.2022
* Author : Martin Stasch (MS)
*
* Animation and sound controller for AI using the AI state machine states.
*
* History:
*	16.02.2022	MS	Created
******************************************************************************/


using UnityEngine;


public class AIAnimationandSoundManager : MonoBehaviour
{
    [Header("Animation Components")]
    [SerializeField]
    private AIScriptableData aIScriptableData = null;
    [SerializeField]
    private Animator aIAnimator = null;

    private string runningParameterName = "Running";
    private int runningHash = 0;

    private void Awake()
    {
        aIAnimator = GetComponent<Animator>();

        runningHash = Animator.StringToHash(runningParameterName);
    }
    void Update()
    {
        if((aIAnimator != null) && (aIScriptableData != null))
        {
            switch (aIScriptableData.AIState)
            {
                case AIScriptableData.EAIBehavior.AISelectNewSearchPointState:
                    break;
                case AIScriptableData.EAIBehavior.AIFindRandomSearchPointState:
                    break;
                case AIScriptableData.EAIBehavior.AIMoveToPositionState:
                    aIAnimator.SetBool(runningHash, true);
                    break;
                case AIScriptableData.EAIBehavior.AIFoundItemState:
                    break;
                case AIScriptableData.EAIBehavior.AIMoveToKeyState:
                    aIAnimator.SetBool(runningHash, true);
                    break;
                case AIScriptableData.EAIBehavior.AIPickUPKeyState:
                    break;
            }

        }
    }
}