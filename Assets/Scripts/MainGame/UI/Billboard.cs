using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        if(Camera.main != null)
            transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
