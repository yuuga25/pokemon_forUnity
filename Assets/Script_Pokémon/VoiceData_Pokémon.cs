using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObject/p_VoiceData")]
public class VoiceData_Pokémon : ScriptableObject
{
    public List<p_VoiceData> sheet;

    [Serializable]
    public class p_VoiceData
    {
        public int p_Id;
        public AudioClip voiceData;
    }
}
