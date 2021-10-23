using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataLists.playerData.user_Name = "aa";
        print(DataLists.playerData.user_Name);
    }
}
