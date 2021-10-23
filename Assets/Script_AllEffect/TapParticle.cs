using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapParticle : MonoBehaviour
{
    public GameObject tapEffect;
    public float deleteTime = 0.3f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 3f;
            GameObject clone = Instantiate(tapEffect, Camera.main.ScreenToWorldPoint(mousePosition), Quaternion.identity);
            Destroy(clone, deleteTime);
        }
    }
}
