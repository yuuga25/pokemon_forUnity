using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObject/b_ImageData")]
public class ImageData_Belongings : ScriptableObject
{
    public List<b_ImageData> sheet;

    [Serializable]
    public class b_ImageData
    {
        public string displayName;
        public string imageId;
        public Sprite item_Image;
    }
}
