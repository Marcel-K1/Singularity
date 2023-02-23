/*****************************************************************************
* Project: Singularity
* File   : GameDifficultySettings.cs
* Date   : 16.02.2022
* Author : Martin Stasch (MS)
*
* Data container for game difficulty setup information.
*
* History:
*	16.02.2022	MS	Created
******************************************************************************/


using UnityEngine;


[CreateAssetMenu(fileName = "GameDifficultySettings", menuName = "ScriptableObjects/GameDifficultySettings")]
public class GameDifficultySettings : ScriptableObject
{
    #region First Round Settings
    [Header("Game Initialisation / First Round Settings")]
    [SerializeField]
    private int keysToCollect = 3;
    [SerializeField, Range(0, 3)]
    private int AditionalKeysPerPlayerMultiplayer = 1; // Adjustment for difficulty higher number means more aditional keys and therfore it is easier to find required keys
    [SerializeField, Range(0.5f, 2.0f)]
    private float aISpeedAdjustmentRound0 = 1f;
    [SerializeField, Range(0.5f, 2.0f)]
    private float aISightAdjustmentRound0 = 1f;
    [SerializeField, Range(3, 7)]
    private int maxGatesInRound0 = 4;
    [SerializeField, Range(4, 8)]
    private int maxPlayerInRound0 = 6;
    
    public int KeysToCollect { get { return keysToCollect; } }
    public float AISpeedAdjustmentRound0 { get { return aISpeedAdjustmentRound0; } }
    public float AISightAdjustmentRound0 { get { return aISightAdjustmentRound0; } }
    public int MaxGatesInRound0 { get { return maxGatesInRound0; } }
    public int MaxPlayerInRound0 { get { return maxPlayerInRound0; } }
    public int MaxKeysInRound0
    {
        get { return (int)(

                // calculation for keys to spawn according to parameters set
                ((keysToCollect - 1) * MaxPlayerInRound0) + MaxGatesInRound0 +
                (AditionalKeysPerPlayerMultiplayer * MaxPlayerInRound0)
                
                );
        }

    }
    #endregion

    #region Second Round Settings
    [Header("Round 2 Settings")]
    [SerializeField, Range(0.5f, 2.0f)]
    private float aISpeedAdjustmentRound1 = 1f;
    [SerializeField, Range(0.5f, 2.0f)]
    private float aISightAdjustmentRound1 = 1f;
    [SerializeField, Range(2, 6)]
    private int maxGatesInRound1 = 2;

    public float AISpeedAdjustmentRound1 { get { return aISpeedAdjustmentRound1; } }
    public float AISightAdjustmentRound1 { get { return aISightAdjustmentRound1; } }
    public int MaxGatesInRound1 { get { return maxGatesInRound1; } }
    public int MaxPlayerInRound1 { get { return MaxGatesInRound0; } }
    public int MaxKeysInRound1
    {
        get
        {
            return (int)(

              // calculation for keys to spawn according to parameters set
              ((keysToCollect - 1) * MaxPlayerInRound1) + MaxGatesInRound1 +
              (AditionalKeysPerPlayerMultiplayer * MaxPlayerInRound1)

              );
        }

    }
    #endregion

    #region Final Round Settings
    [Header("Final Round Settings")]
    [SerializeField, Range(0.5f, 2.0f)]
    private float aISpeedAdjustmentRound2 = 1f;
    [SerializeField, Range(0.5f, 2.0f)]
    private float aISightAdjustmentRound2 = 1f;
    private int maxGatesInRound2 = 1;

    public float AISpeedAdjustmentRound2 { get { return aISpeedAdjustmentRound2; } }
    public float AISightAdjustmentRound2 { get { return aISightAdjustmentRound2; } }
    public int MaxGatesInRound2 { get { return maxGatesInRound2; } }
    public int MaxPlayerInRound2 { get { return MaxGatesInRound1; } }
    public int MaxKeysInRound2
    {
        get
        {
            return (int)(

              // calculation for keys to spawn according to parameters set
              ((keysToCollect - 1) * MaxPlayerInRound2) + MaxGatesInRound2 +
              (AditionalKeysPerPlayerMultiplayer * MaxPlayerInRound2)

              );
        }

    }
    #endregion
}
