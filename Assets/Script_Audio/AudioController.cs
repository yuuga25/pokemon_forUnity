using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public enum AudioType
    {
        BGM,
        SE,
        Voice
    }

    public AudioType audioType;

    private float masterVolume;

    public static float Master_Volume = 1f;
    public static float BGM_Volume = 0.08f;
    public static float SE_Volume = 0.1f;
    public static float Voice_Volume = 0.1f;

    private void Start()
    {
        masterVolume = Master_Volume;
    }

    private void Update()
    {
        if(audioType == AudioType.BGM && this.gameObject.GetComponent<AudioSource>().volume != BGM_Volume || masterVolume != Master_Volume)
        {
            var audio = this.gameObject.GetComponent<AudioSource>();
            audio.volume = BGM_Volume * Master_Volume;
            masterVolume = Master_Volume;
        }
        else if(audioType == AudioType.SE && this.gameObject.GetComponent<AudioSource>().volume != SE_Volume || masterVolume != Master_Volume)
        {
            var audio = this.gameObject.GetComponent<AudioSource>();
            audio.volume = SE_Volume * Master_Volume;
            masterVolume = Master_Volume;
        }
        else if(audioType == AudioType.Voice && this.gameObject.GetComponent<AudioSource>().volume != Voice_Volume || masterVolume != Master_Volume)
        {
            var audio = this.gameObject.GetComponent<AudioSource>();
            audio.volume = Voice_Volume * Master_Volume;
            masterVolume = Master_Volume;
        }
    }
}
