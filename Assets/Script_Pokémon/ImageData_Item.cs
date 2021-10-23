using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObject/i_ImageData")]
public class ImageData_Item : ScriptableObject
{
    public List<i_ImageData> sheet;

    [Serializable]
    public class i_ImageData
    {
        public string imageId;
        public Sprite item_Image;
    }
}
