/********************************************************************************************
* Project: Singularity
* File   : Audio Zone
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Is used for playing different ambient sounds, when the player enters the attached collider.
* 
* History:
*    09.02.2022    MK    Created
**********************************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioZone : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource = null;

    private bool alreadyPlayed = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!alreadyPlayed && !audioSource.isPlaying)
            {
                audioSource.Play();
                audioSource.loop = true;
            }
            else if (alreadyPlayed && !audioSource.isPlaying)
            {
                audioSource.UnPause();
                audioSource.loop = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioSource.Pause();
            alreadyPlayed = true;
        }
    }

}
