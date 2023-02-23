/*******************************************************************************
* Project: Singularity
* File   : Credit Manu Manager
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Manages canvas for the credit menu in the win and loose scene.
* 
* History:
*    02.03.2022    MK    Created
*******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CreditMenuManager : MonoBehaviour
{
    [SerializeField]
    private Transform firstPanel = null;
    [SerializeField]
    private Transform secondPanel = null;

    private Stack<Transform> activePanels = null;

    private void Awake()
    {
        Time.timeScale = 1f;
        activePanels = new Stack<Transform>();
        activePanels.Push(firstPanel);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Start()
    {
        StartCoroutine(WaitForSeconds()); 
    }

    #region Button Methods
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
        Transform currentPanel = activePanels.Peek();
        currentPanel.gameObject.SetActive(false);
        activePanels.Push(_next);
        _next.gameObject.SetActive(true);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(5);
        ShowNext(secondPanel);
    }
    public void OpenChannel()
    {
        Application.OpenURL("https://sites.google.com/view/singularitygame");
    }
    #endregion
}
