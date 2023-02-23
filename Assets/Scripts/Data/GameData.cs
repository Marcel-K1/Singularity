/**********************************************************************************************
* Project: Singularity
* File   : GM Data
* Date   : 09.02.2022
* Author : Martin Stasch & Marcel Klein
*
* Manages the important data for the game to run. Set up as a scriptable object for easy implemantation and saving the data over different game rounds.
* 
* History:
*    01.03.2022    MS & MK    Created
**********************************************************************************************/


using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    public enum EDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    [SerializeField]
    private EDifficulty playerDifficulty = 0;
    public EDifficulty PlayerDifficulty { get => playerDifficulty;}

    [Header("Game Data")]
    [SerializeField]
    private int singularitiesCount = 0;
    public int SingularitiesCount { get => singularitiesCount; set => singularitiesCount = value; }
    [SerializeField]
    private int keyCount = 0;
    public int KeyCount { get => keyCount; set => keyCount = value; }
    [SerializeField]
    private int singularitiesLeft = 0;
    public int SingularitiesLeft { get => singularitiesLeft; set => singularitiesLeft = value; }
    [SerializeField]
    private int keysGathered = 0;
    public int KeysGathered { get => keysGathered; set => keysGathered = value; }
    [SerializeField]
    private int playerCount = 1;
    public int PlayerCount { get => playerCount; set => playerCount = value; }
    [SerializeField]
    private int gameRound = 0;
    public int GameRound { get => gameRound; set => gameRound = value; }
    [SerializeField]
    private int playerLeft = 0;
    public int PlayerLeft { get => playerLeft; set => playerLeft = value; }

    [Header("Memory for players who used a gate")]
    [SerializeField]
    public Stack<GameObject> playerList = new Stack<GameObject>();


    public void SetGameDataToLevelStart()
    {
        playerLeft = playerCount;
        keysGathered = 0;
    }

    public void SetDefaultDifficulty()
    {
        playerDifficulty = EDifficulty.Easy;
    }

    public void UpdateGameDifficulty(int _difficulty)
    {
        playerDifficulty = (EDifficulty)_difficulty;
    }

    /// <summary>
    /// Updating player data method for level manager to display necessary game data.
    /// </summary>
    /// <param name="_playerId"></param>
    public void PlayerUpdateGameData(GameObject _playerId)
    {
        singularitiesLeft--;
        playerLeft--;
        playerList.Push(_playerId);
    }

    /// <summary>
    /// Updating AI data method for level manager to display necessary game data for the player and to be able to check for next round.
    /// </summary>
    public void AIUpdateGameData()
    {
        singularitiesLeft--;
    }


}
