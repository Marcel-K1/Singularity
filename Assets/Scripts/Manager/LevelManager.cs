/*******************************************************************************
* Project: Singularity
* File   : Level Manager
* Date   : 09.02.2022
* Author : Martins Stasch & Marcel Klein
*
* Manages the different levels. Changes the important values for different difficulty settings. Restarts Levels and Spawns AI accordingly to settings.
* 
* History:
*    03.03.2022    MS & MK    Created
*******************************************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get { return instance; }
    }

    public int SingularitiesLeft { get => data.SingularitiesLeft; }
    public int KeysGathered { get => data.KeysGathered; }

    private int keysToCollect = 3;
    public int KeysToCollect { get => keysToCollect; }

    private event Action<int, GameObject> spawnGateEvent = (gateobjectToSpawn, gatePrefab) => { };
    public event Action<int, GameObject> SpawnGateEvent { add => spawnGateEvent += value; remove => spawnGateEvent -= value; }

    private event Action<int, GameObject> spawnKeyEvent = (keyobjectToSpawn, keyPrefab) => { };
    public event Action<int, GameObject> SpawnKeyEvent { add => spawnKeyEvent += value; remove => spawnKeyEvent -= value; }

    private event Action levelResetEvent = () => { };
    public event Action LevelResetEvent { add => levelResetEvent += value; remove => levelResetEvent -= value; }

    public event Action roundStartEvent = () => { };
    public event Action RoundStartEvent { add => roundStartEvent += value; remove => roundStartEvent -= value; }


    [Header("Required Info")]
    [SerializeField]
    private GameData data = null;
    [SerializeField]
    private PlayerController playerController = null;
    [SerializeField]
    private GameDifficultySettings gameDSetEasy = null;
    [SerializeField]
    private GameDifficultySettings gameDSetNormal = null;
    [SerializeField]
    private GameDifficultySettings gameDSetHard = null;

    [Header("UI Info")]
    [SerializeField]
    private GameObject pauseMenuUI = null;
    [SerializeField]
    private Transform firstPanel = null;
    [SerializeField]
    private Stack<Transform> activePanels = null;

    [Header("Spawner Info")]
    [SerializeField]
    private GameObject gatePrefab = null;
    [SerializeField]
    private GameObject keyPrefab = null;
    [SerializeField]
    private List<Transform> charSP = new List<Transform>();
    [SerializeField]
    private List<GameObject> playerList = new List<GameObject>();

    [Header("AI Manager Data")]
    [SerializeField]
    List<AIScriptableData> aiDataList = null;


    #region PauseMenuMethods

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void LoadMainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }
    public void ShowNext(Transform _next)
    {
        Transform currentPanel = activePanels.Peek();
        currentPanel.gameObject.SetActive(false);
        activePanels.Push(_next);
        _next.gameObject.SetActive(true);
    }
    public void Close()
    {
        Transform panel = activePanels.Pop();
        panel.gameObject.SetActive(false);
        Transform currentPanel = activePanels.Peek();
        currentPanel.gameObject.SetActive(true);
    }
    public void Resume()
    {
        GameManager.Instance.Pause(false);
        pauseMenuUI.SetActive(false);
        playerController.LockCursor = true;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameManager.Instance.Pause(true);
    }

    #endregion

    #region LevelMethods

    /// <summary>
    /// Initializes the AI at the beginning of the game and assignes the game difficulty adjustments to default settings
    /// </summary>
    /// <param name="_difficulty"></param>
    /// <param name="_playercount"></param>
    void GameDifficultySetup(GameData.EDifficulty _difficulty, int _playercount, int _gameRound)
    {

        switch (_difficulty)
        {
            case GameData.EDifficulty.Easy:
                {
                    if (_gameRound == 0)
                    { 
                        keysToCollect = gameDSetEasy.KeysToCollect;
                        ActivateAIForGame(gameDSetEasy.AISpeedAdjustmentRound0, gameDSetEasy.AISightAdjustmentRound0,
                            gameDSetEasy.MaxPlayerInRound0, _playercount); 
                        InitialPlayerActivation(_playercount);
                        spawnGateEvent?.Invoke(gameDSetEasy.MaxGatesInRound0, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetEasy.MaxKeysInRound0, keyPrefab);
                        data.SingularitiesCount = gameDSetEasy.MaxGatesInRound0;
                        data.SingularitiesLeft = gameDSetEasy.MaxGatesInRound0;
                        data.KeyCount = gameDSetEasy.MaxKeysInRound0;
                        roundStartEvent?.Invoke();
                        break;
                    }
                    else if(_gameRound == 1)
                    {
                        ActivateAIForGame(gameDSetEasy.AISpeedAdjustmentRound1, gameDSetEasy.AISightAdjustmentRound1,
                            gameDSetEasy.MaxPlayerInRound1, _playercount);
                        ActivatePlayerForNextLevel();
                        spawnGateEvent?.Invoke(gameDSetEasy.MaxGatesInRound1, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetEasy.MaxKeysInRound1, keyPrefab);
                        data.SingularitiesCount = gameDSetEasy.MaxGatesInRound1;
                        data.SingularitiesLeft = gameDSetEasy.MaxGatesInRound1;
                        data.KeyCount = gameDSetEasy.MaxKeysInRound1;
                        roundStartEvent?.Invoke();
                        break;
                    }
                    else
                    {
                        ActivateAIForGame(gameDSetEasy.AISpeedAdjustmentRound2, gameDSetEasy.AISightAdjustmentRound2,
                            gameDSetEasy.MaxPlayerInRound2, _playercount);
                        ActivatePlayerForNextLevel();
                        spawnGateEvent?.Invoke(gameDSetEasy.MaxGatesInRound2, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetEasy.MaxKeysInRound2, keyPrefab);
                        data.SingularitiesCount = gameDSetEasy.MaxGatesInRound2;
                        data.SingularitiesLeft = gameDSetEasy.MaxGatesInRound2;
                        data.KeyCount = gameDSetEasy.MaxKeysInRound2;
                        roundStartEvent?.Invoke();
                        break;
                    }
                }
            case GameData.EDifficulty.Normal:
                {
                    if (_gameRound == 0)
                    {
                        keysToCollect = gameDSetNormal.KeysToCollect;
                        ActivateAIForGame(gameDSetNormal.AISpeedAdjustmentRound0, gameDSetNormal.AISightAdjustmentRound0,
                            gameDSetNormal.MaxPlayerInRound0, _playercount);
                        InitialPlayerActivation(_playercount);
                        spawnGateEvent?.Invoke(gameDSetNormal.MaxGatesInRound0, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetNormal.MaxKeysInRound0, keyPrefab);
                        data.SingularitiesCount = gameDSetNormal.MaxGatesInRound0;
                        data.SingularitiesLeft = gameDSetNormal.MaxGatesInRound0;
                        data.KeyCount = gameDSetNormal.MaxKeysInRound0;
                        roundStartEvent?.Invoke();
                        break;
                    }
                    else if (_gameRound == 1)
                    {
                        ActivateAIForGame(gameDSetNormal.AISpeedAdjustmentRound1, gameDSetNormal.AISightAdjustmentRound1,
                            gameDSetNormal.MaxPlayerInRound1, _playercount);
                        ActivatePlayerForNextLevel();
                        spawnGateEvent?.Invoke(gameDSetNormal.MaxGatesInRound1, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetNormal.MaxKeysInRound1, keyPrefab);
                        data.SingularitiesCount = gameDSetNormal.MaxGatesInRound1;
                        data.SingularitiesLeft = gameDSetNormal.MaxGatesInRound1;
                        data.KeyCount = gameDSetNormal.MaxKeysInRound1;
                        roundStartEvent?.Invoke();
                        break;
                    }
                    else
                    {
                        ActivateAIForGame(gameDSetNormal.AISpeedAdjustmentRound2, gameDSetNormal.AISightAdjustmentRound2,
                            gameDSetNormal.MaxPlayerInRound2, _playercount);
                        ActivatePlayerForNextLevel();
                        spawnGateEvent?.Invoke(gameDSetNormal.MaxGatesInRound2, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetNormal.MaxKeysInRound2, keyPrefab);
                        data.SingularitiesCount = gameDSetNormal.MaxGatesInRound2;
                        data.SingularitiesLeft = gameDSetNormal.MaxGatesInRound2;
                        data.KeyCount = gameDSetNormal.MaxKeysInRound2;
                        roundStartEvent?.Invoke();
                        break;
                    }
                }

            case GameData.EDifficulty.Hard:
                {
                    if (_gameRound == 0)
                    {
                        keysToCollect = gameDSetHard.KeysToCollect;
                        ActivateAIForGame(gameDSetHard.AISpeedAdjustmentRound0, gameDSetHard.AISightAdjustmentRound0,
                            gameDSetHard.MaxPlayerInRound0, _playercount);
                        InitialPlayerActivation(_playercount);
                        spawnGateEvent?.Invoke(gameDSetHard.MaxGatesInRound0, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetHard.MaxKeysInRound0, keyPrefab);
                        data.SingularitiesCount = gameDSetHard.MaxGatesInRound0;
                        data.SingularitiesLeft = gameDSetHard.MaxGatesInRound0;
                        data.KeyCount = gameDSetHard.MaxKeysInRound0;
                        roundStartEvent?.Invoke();
                        break;
                    }
                    else if (_gameRound == 1)
                    {
                        ActivateAIForGame(gameDSetHard.AISpeedAdjustmentRound1, gameDSetHard.AISightAdjustmentRound1,
                            gameDSetHard.MaxPlayerInRound1, _playercount);
                        ActivatePlayerForNextLevel();
                        spawnGateEvent?.Invoke(gameDSetHard.MaxGatesInRound1, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetHard.MaxKeysInRound1, keyPrefab);
                        data.SingularitiesCount = gameDSetHard.MaxGatesInRound1;
                        data.SingularitiesLeft = gameDSetHard.MaxGatesInRound1;
                        data.KeyCount = gameDSetHard.MaxKeysInRound1;
                        roundStartEvent?.Invoke();
                        break;
                    }
                    else
                    {
                        ActivateAIForGame(gameDSetHard.AISpeedAdjustmentRound2, gameDSetHard.AISightAdjustmentRound2,
                            gameDSetHard.MaxPlayerInRound2, _playercount);
                        ActivatePlayerForNextLevel();
                        spawnGateEvent?.Invoke(gameDSetHard.MaxGatesInRound2, gatePrefab);
                        spawnKeyEvent?.Invoke(gameDSetHard.MaxKeysInRound2, keyPrefab);
                        data.SingularitiesCount = gameDSetHard.MaxGatesInRound2;
                        data.SingularitiesLeft = gameDSetHard.MaxGatesInRound2;
                        data.KeyCount = gameDSetHard.MaxKeysInRound2;
                        roundStartEvent?.Invoke();
                        break;
                    }
                }
            default:
                break;
        }
    }

    /// <summary>
    /// Generates a random respawnpoint from spawnpoints list
    /// </summary>
    /// <returns></returns>
    public Transform FindRandomCharSP()
    {

        if (charSP == null || charSP.Count == 0)
        {
            foreach (GameObject GO in GameObject.FindGameObjectsWithTag("CharSP"))
            {
                charSP.Add(GO.transform);
            };
        }

        Transform randomCharSP;
        randomCharSP = charSP[UnityEngine.Random.Range(0, charSP.Count)];
        return randomCharSP;
    }

    /// <summary>
    /// AI Default, Game Data Default, Keys and Gates Default, Deactivate Inactive Player
    /// </summary>
    private void ResetGameLevel()
    {
        ResetAIDataToDefault();
        DeactivatePlayer();
        data.SetGameDataToLevelStart();
        levelResetEvent?.Invoke();
    }


    #endregion

    #region PlayerMethods

    /// <summary>
    /// Deactivate all player in the scene to ensure a clean game / round start
    /// </summary>
    void DeactivatePlayer()
    {
        foreach (GameObject player in playerList)
        {
            player.SetActive(false);
        }
    }

    /// <summary>
    /// First activation of player at game level start 
    /// </summary>
    /// <param name="_playercount"></param>
    void InitialPlayerActivation(int _playercount)
    {
        for (int i = 0; i < _playercount; i++)
        {
            playerList[i].SetActive(true);
            playerList[i].transform.position = FindRandomCharSP().position;
        }
    }

    /// <summary>
    /// Activation of player for round 2 and 3
    /// </summary>
    void ActivatePlayerForNextLevel()
    {
        do
        {
            GameObject player = data.playerList.Pop();
            player.transform.position = FindRandomCharSP().position;
            player.SetActive(true);

        } while(data.playerList.Count > 0);
    }

    #endregion

    #region AiMethods


    /// <summary>
    /// activates the game AI for a match
    /// </summary>
    /// <param name="_difficultyValue"></param>
    /// <param name="_maxPlayer"></param>
    /// <param name="_playercount"></param>
    void ActivateAIForGame(float _aISpeedAdjustment, float _aISightAdjustment, int _maxPlayer, int _playercount)
    {
        // GameManager provides Data on Player - 1 to 4 ; AI to activate is 8 - playercount = 4 to 7 
        for (int i = 0; i < (_maxPlayer - _playercount); i++)
        {
            aiDataList[i].AIId.gameObject.transform.position = FindRandomCharSP().position;
            aiDataList[i].AIId.gameObject.SetActive(true);
            aiDataList[i].SetDifficultyValues(_aISpeedAdjustment, _aISightAdjustment);
        }
    }

    /// <summary>
    /// Resets AI to default state and inactive 
    /// </summary>
    void ResetAIDataToDefault()
    {
        foreach (AIScriptableData aiData in aiDataList)
        {
            aiData.ResetToDefault();
        }
    }

    #endregion

    #region Win/Lose Condition checks for AI and Player

    /// <summary>
    /// Checks if player used a gate and if the game round is therefore completed 
    /// (all games used up/no player in the round) 
    /// and the game can be restarted or marked as won in case of the last round
    /// </summary>
    /// <param name="_playerId"></param>
    public void CheckForPlayer(GameObject _playerId)
    {
        data.PlayerUpdateGameData(_playerId);
        if (data.GameRound != 2)
        {

            if (data.PlayerLeft == 0)
            {
                data.GameRound++;
                data.PlayerLeft = data.PlayerCount;
                data.playerList.Push(_playerId);
                ResetGameLevel();
                GameDifficultySetup(data.PlayerDifficulty, data.PlayerCount, data.GameRound);
            }
            else if (data.SingularitiesLeft == 0)
            {
                // currently unused case!

                data.GameRound++;
                data.playerList.Push(_playerId);

                ResetGameLevel();
                GameDifficultySetup(data.PlayerDifficulty, data.PlayerCount, data.GameRound);
            }
            else
            {
                data.playerList.Push(_playerId);
            }

        }
        else
        {
            GameManager.Instance.LoadWinScene();
        }
    }


    /// <summary>
    /// Checks if AI used a gate and in case of last gate used the next round is started 
    /// or if no player in the round was unable to use a gate the lose condition is triggered
    /// </summary>
    public void CheckForAI()
    {
        data.AIUpdateGameData();
        if (data.SingularitiesLeft == 0)
        {
            if (data.PlayerLeft == data.PlayerCount)
            {
                GameManager.Instance.LoadLooseScene();
            }
            else
            {
                data.GameRound++;

                ResetGameLevel();
                GameDifficultySetup(data.PlayerDifficulty, data.PlayerCount, data.GameRound);
            }

        }
    }

    #endregion


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
            instance = this;

        playerController = FindObjectOfType<PlayerController>();

        activePanels = new Stack<Transform>();
        activePanels.Push(firstPanel);
    }
    private void Start()
    {
        //Reset Data
        ResetAIDataToDefault();
        DeactivatePlayer();
        data.SetGameDataToLevelStart();

        //Setup Level
        GameDifficultySetup(data.PlayerDifficulty, data.PlayerCount, data.GameRound) ;
    }
    private void Update()
    {
        //Check for Pause
        if (InputManager.Instance.Pause == 1)
        {
            playerController.LockCursor = false;
            Pause();
        }

    }
}
