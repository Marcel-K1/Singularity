/*******************************************************************************
* Project: Singularity
* File   : Main Menu Manager
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Manages canvas for the main menu. Takes input for setting up volume and difficulty.
* 
* History:
*    02.02.2022    MK    Created
*******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_firstPanel = null;
    [SerializeField]
    private GameData data = null;

    private Stack<Transform> m_activePanels = null;

    private int playerDifficulty = 0;
    public int PlayerDifficulty { get => playerDifficulty; set => playerDifficulty = value; }


    private void Awake()
    {
        m_activePanels = new Stack<Transform>();
        m_activePanels.Push(m_firstPanel);
    }
    private void Start()
    {
        data.SetDefaultDifficulty();
        StartCoroutine(CameraSystem(2f));
    }
    private void Update()
    {
        data.UpdateGameDifficulty(playerDifficulty);
    }

    #region Button Methods
    public void LoadScene()
    {
        data.GameRound = 0;
        GameManager.Instance.LoadLevel();
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void ShowNext(Transform _next)
    {
        Transform currentPanel = m_activePanels.Peek();
        currentPanel.gameObject.SetActive(false);
        m_activePanels.Push(_next);
        _next.gameObject.SetActive(true);
    }
    public void Close()
    {
        Transform panel = m_activePanels.Pop();
        panel.gameObject.SetActive(false);
        Transform currentPanel = m_activePanels.Peek();
        currentPanel.gameObject.SetActive(true);
    }
    public void SetDifficulty(int _difficulty)
    {
        playerDifficulty = _difficulty;
    }
    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
    #endregion

    private IEnumerator CameraSystem(float _time)
    {
        Camera camera = Camera.main;
        camera.gameObject.SetActive(false);
        yield return new WaitForSeconds(_time);
        camera.gameObject.SetActive(true);
    }

}


