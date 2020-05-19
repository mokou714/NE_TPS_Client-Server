using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private Transform mask;
    private float _defaultX;
    private void Start()
    {
        mask = transform.GetChild(0);
        _defaultX = mask.localPosition.x;
    }

    public void LostHealthFraction(float fraction)
    {
        var diff = new Vector3(-_defaultX*fraction, 0f, 0f );
        if (mask.localPosition.x + diff.x > 0)
        {
            diff.x =  -mask.localPosition.x;
        }
        mask.localPosition += diff;
    }
}
