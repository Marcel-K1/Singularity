/*******************************************************************************
* Project: Singularity
* File   : Panel Text
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Synchronizes SingularitiesLeft variable from GM Data with ingame global display.
* 
* History:
*    01.03.2022    MK    Created
*******************************************************************************/


using TMPro;
using UnityEngine;


public class PanelText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro singularitiesLeftPanel;


    void Awake()
    {
        singularitiesLeftPanel = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        singularitiesLeftPanel.text = LevelManager.Instance.SingularitiesLeft.ToString();
    }

}
