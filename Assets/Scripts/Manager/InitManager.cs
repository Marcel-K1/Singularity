/*******************************************************************************
* Project: Singularity
* File   : Init Manager
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Initializes the the main menu. 
* 
* History:
*    22.02.2022    MK    Created
*******************************************************************************/


using UnityEngine;


public class InitManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private void Awake()
    {
        gameManager.LoadMainMenu();
    }
}
