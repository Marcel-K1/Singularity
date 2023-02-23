/*
* Project: Singularity
* File   : HUD Manager
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Manages the Heads Up Display for the player. Displaying SingularitiesLeft member and KeyGathered member from GM Data.
* 
* History:
*    22.02.2022    MK    Created
*/


using System.Collections;
using TMPro;
using UnityEngine;


public class HUDManager : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField]
    private TextMeshProUGUI gameDataHud = null;
    [SerializeField]
    private TextMeshProUGUI gameRoundHud = null;
     
    [SerializeField]
    private PlayerController playerController = null;
    [SerializeField]
    private GameData data = null;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    void Update()
    {
        gameDataHud.text = "Singularities left: " + data.SingularitiesLeft + 
            "\r\nKeys gathered: " + data.KeysGathered + 
            "\r\nPress P to pause the game!";

        if (playerController.CollisionWithKey)
        {
            gameDataHud.text = "Singularities left: " + data.SingularitiesLeft + 
                "\r\nKeys gathered: " + data.KeysGathered + 
                "\r\nPlease press E to gather the key!";
        }
        else if (playerController.CollisionWithGate)
        {
            gameDataHud.text = "Singularities left: " + data.SingularitiesLeft + 
                "\r\nKeys gathered: " + data.KeysGathered + 
                "\r\nPlease press E to use the Singularity!";
        }
        else if (playerController.CollisionWithGateError)
        {
            gameDataHud.text = "Singularities left: " + data.SingularitiesLeft + 
                "\r\nKeys gathered: " + data.KeysGathered + 
                "\r\nSorry, you first have to gather 3 keys!";
        }
        else
        {
            gameDataHud.text = "Singularities left: " + data.SingularitiesLeft +
            "\r\nKeys gathered: " + data.KeysGathered +
            "\r\nPress P to pause the game!";
        }
    }

    //Method to display instructions for the player at level start triggered by event in level manager.
    private void DisplayCurrentRound()
    {
        StartCoroutine(DisplayCurrentRoundAndDeactivate(5f));
    }
    private IEnumerator DisplayCurrentRoundAndDeactivate(float _time)
    {
        gameRoundHud.gameObject.SetActive(true);
        switch (data.GameRound)
        {
            case 0:
                gameRoundHud.text = "ROUND: 1\r\n Find " 
                    + LevelManager.Instance.KeysToCollect + " keys to enter the Singularity!";
                break;
            case 1:
                gameRoundHud.text = "ROUND: 2\r\n Find " 
                    + LevelManager.Instance.KeysToCollect + " keys to enter the Singularity!";
                break;
            case 2:
                gameRoundHud.text = "FINAL ROUND\r\n Find " 
                    + LevelManager.Instance.KeysToCollect + " keys to enter the Singularity!";
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(_time);
        gameRoundHud.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RoundStartEvent += DisplayCurrentRound;
        }
    }
    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RoundStartEvent -= DisplayCurrentRound;
        }
    }
}
