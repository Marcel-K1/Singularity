/*******************************************************************************
* Project: Singularity
* File   : Game Manager
* Date   : 09.02.2022
* Author : Martin Stasch & Marcel Klein
*
* Manages the whole game as a singleton.
* 
* History:
*    01.03.2022    MS & MK    Created
*******************************************************************************/


using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #region Administration Methods
    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Init");
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level1Scene");
    }
    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }    
    public void LoadLooseScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("LooseScene");
    }
    public void Pause(bool _paused)
    {
        if (_paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    #endregion
}
