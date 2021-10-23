using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObject/p_ImageData")]
public class ImageData_Pokémon : ScriptableObject
{
    public List<p_ImageData> sheet;
    public enum GenderType
    {
        same,
        maleOnly,
        femeleOnly
    }

    [Serializable]
    public class p_ImageData
    {
        public string p_Name;
        public int p_Id;
        public GenderType genderType;
        public Sprite p_ImageBack;
        public Sprite p_ImageBack_C;
        public Sprite p_ImageFront;
        public Sprite p_ImageFront_C;
        public Sprite p_ImageHand;
        public Sprite p_ImageHand_C;
    }
}
