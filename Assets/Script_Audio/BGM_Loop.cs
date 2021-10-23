using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Loop : MonoBehaviour
{
    public AudioClip intro;

    public AudioSource audioManager_BGM;

    private void Start()
    {
        audioManager_BGM.PlayScheduled(AudioSettings.dspTime + intro.length);
    }
}
