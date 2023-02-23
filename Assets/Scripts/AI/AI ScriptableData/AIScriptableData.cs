/*****************************************************************************
* Project: Singularity
* File   : AIScriptableData.cs
* Date   : 09.02.2022
* Author : Martin Stasch (MS)
*
* Data container for AI logic and AI state machine control.
*
* History:
*	09.02.2022	MS	Created
******************************************************************************/


using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "AIScriptableData", menuName = "ScriptableObjects/AIScriptableData")]
public class AIScriptableData : ScriptableObject
{
    public enum EAIBehavior
    {
        AISelectNewSearchPointState,
        AIFindRandomSearchPointState,
        AIMoveToPositionState,
        AIFoundItemState,
        AIMoveToKeyState,
        AIPickUPKeyState,
        AISelectGatePositionState,
        AIMoveToGatePositionState,
        AIUseGateState,
        AISearchForGateState,
    }

    [Header("AI Individual Default Settings")]
    [SerializeField, Range(6, 15)]
    private float aIRunSpeedDefault = 6f;
    [SerializeField, Range(6, 15)]
    private float aISightRangeDefault = 6f;
    [SerializeField, Range(5, 15)]
    private int maxSearchAtSearchPointAttemptsDefault = 8;
    [SerializeField, Range(1, 5)]
    private int minSearchAtSearchPointAttemptsDefault = 3;
    [SerializeField, Range(10f, 25f)]
    private float walkPointRangeDefault = 20f;
    [SerializeField, Range(0f, 2f)]
    private float walkPointRangeHeightDefault = 0.5f;
    [SerializeField, Range(0f, 3f)]
    private float distanceToWalkMagnitudeCheckDefault = 1.5f;

    [Header("AI Identifier")]
    private Transform aIId = null;
    public Transform AIId { get => aIId; set => aIId = value; }

    [Header("Search Logic Parameters")]
    [SerializeField]
    private Vector3 searchPoint = new Vector3();
    [SerializeField]
    private Vector3 randomSearchPointAtSearchPoint = new Vector3();
    private Collider foundItemCollider = null;
    private float walkPointRange = 20f;
    private float walkPointRangeHeight = 0.5f;
    private float distanceToWalkMagnitudeCheck = 1.5f;
    private int maxSearchAtSearchPointAttempts = 8;
    private int minSearchAtSearchPointAttempts = 3;
    [SerializeField]
    private float aIRunSpeed = 4f;
    [SerializeField]
    private int foundKeys = 0;
    [SerializeField]
    private int requiredKeysToUnlockGate = 3;

    public int FoundKeys { get => foundKeys; set => foundKeys = value; }
    public int RequiredKeysToUnlockGate { get => requiredKeysToUnlockGate; 
        set => requiredKeysToUnlockGate = value; }
    public Vector3 SearchPoint { get => searchPoint; set => searchPoint = value; }
    public Vector3 RandomSearchPointAtSearchPoint { get => randomSearchPointAtSearchPoint; 
        set => randomSearchPointAtSearchPoint = value; }
    public Collider FoundItemCollider { get => foundItemCollider; set => foundItemCollider = value; }
    public float WalkPointRange { get => walkPointRange; set => walkPointRange = value; }
    public float WalkPointRangeHeight { get => walkPointRangeHeight; set => walkPointRangeHeight = value; }
    public float DistanceToWalkMagnitudeCheck { get => distanceToWalkMagnitudeCheck; 
        set => distanceToWalkMagnitudeCheck = value; }
    public int MaxSearchAtSearchPointAttempts { get => maxSearchAtSearchPointAttempts; 
        set => maxSearchAtSearchPointAttempts = value; }
    public int MinSearchAtSearchPointAttempts { get => minSearchAtSearchPointAttempts; 
        set => minSearchAtSearchPointAttempts = value; }
    public float AIRunSpeed { get => aIRunSpeed; set => aIRunSpeed = value; }

    [Header("AI Memory")]
    [SerializeField]
    private Stack<Collider> foundGateLocation = new Stack<Collider>();
    public Stack<Collider> FoundGateLocation { get => foundGateLocation; set => foundGateLocation = value; }

    [Header("AI StateMachine")]
    [SerializeField]
    private EAIBehavior aIState = 0;
    public EAIBehavior AIState { get => aIState ; set => aIState = value; }

    [SerializeField]
    private SphereCollider coll = null;
    public SphereCollider Coll { get => coll; set => coll = value; }


    /// <summary>
    /// restors AI default Status
    /// </summary>
    public void ResetToDefault()
    {
        AIState = EAIBehavior.AISelectNewSearchPointState;
        foundItemCollider = null;
        foundKeys = 0;
        aIId.gameObject.SetActive(false);

        walkPointRange = walkPointRangeDefault;
        walkPointRangeHeight = walkPointRangeHeightDefault;
        distanceToWalkMagnitudeCheck = distanceToWalkMagnitudeCheckDefault;
        maxSearchAtSearchPointAttempts = maxSearchAtSearchPointAttemptsDefault;
        minSearchAtSearchPointAttempts = minSearchAtSearchPointAttemptsDefault;
        aIRunSpeed = aIRunSpeedDefault;
        coll.radius = aISightRangeDefault;
        requiredKeysToUnlockGate = LevelManager.Instance.KeysToCollect;
    }

    /// <summary>
    /// Adjustments of AI variables for selected game difficulty
    /// </summary>
    /// <param name="_speedChange"></param>
    /// <param name="_sightChange"></param>
    public void SetDifficultyValues(float _speedChange, float _sightChange)
    {
        aIRunSpeed = aIRunSpeedDefault * _speedChange;
        coll.radius = aISightRangeDefault * _sightChange;
        requiredKeysToUnlockGate = LevelManager.Instance.KeysToCollect;
    }

}
