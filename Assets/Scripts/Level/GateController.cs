/*******************************************************************************
* Project: Singularity
* File   : Gate Controller
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Controls animation and sound for the gate object.
* 
* History:
*    01.03.2022    MK    Created
*******************************************************************************/


using UnityEngine;


public class GateController : MonoBehaviour
{
    [SerializeField]
    private GameObject spotAnim = null;
    [SerializeField]
    private GameObject gateAnim = null;
    [SerializeField]
    private AudioSource gateAudioSource = null;

    //Property to trigger animation and sound change from player player controller.
    public string Tag
    {
        get { return gameObject.tag; }
        set
        {
            PlaySound();
            spotAnim.gameObject.SetActive(false);
            gateAnim.gameObject.SetActive(true);
            gameObject.tag = value;
        }
    }


    private void Awake()
    {
        gateAudioSource = GetComponent<AudioSource>();  
    }
    
    private void Update()
    {
        if (gameObject.tag == "Gate")
        {
            spotAnim.gameObject.SetActive(true);
        }
    }

    private void PlaySound()
    {
        gateAudioSource.Play();
    }

    //Method to delete obsolete objects
    private void DestroyLeftOverObjects()
    {
        Destroy(gameObject);
    }
    //Apply methods to event 
    private void OnEnable()
    {
        LevelManager.Instance.LevelResetEvent += DestroyLeftOverObjects;
    }
    private void OnDisable()
    {
        LevelManager.Instance.LevelResetEvent -= DestroyLeftOverObjects;
    }
}
