/*******************************************************************************
* Project: Singularity
* File   : Key Controller
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Controls animation and sounds from key object.
* 
* History:
*    01.03.2022    MK    Created
*******************************************************************************/


using UnityEngine;


public class KeyController : MonoBehaviour
{
    [SerializeField]
    private AudioSource keyAudioSource = null;

    [SerializeField]
    private Animator animator = null;

    //Property to change the animation and sound from playercontroller
    public string Tag
    {
        get { return gameObject.tag; }
        set
        {
            ChangeAnimation();
            PlaySound();
            gameObject.tag = value;
        }
    }

    private void Awake()
    {
        keyAudioSource = GetComponent<AudioSource>();
    }
    
    private void ChangeAnimation()
    {
        animator.speed = 8;
    }
    private void PlaySound()
    {
        keyAudioSource.Play();
    }

    //Method to delete obsolete objects
    private void DestroyLeftOverObjects()
    {
        Destroy(gameObject);
    }

    //Apply method to event
    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LevelResetEvent += DestroyLeftOverObjects;
        }
        else
            return;
    }
    private void OnDisable()
    {
        LevelManager.Instance.LevelResetEvent -= DestroyLeftOverObjects;
    }
}
