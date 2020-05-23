using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : ClickableText
{
    [SerializeField] private GameObject connectWindow;
    protected override void OnMouseUp()
    {
        if(isClickable && mouseDown) 
            connectWindow.SetActive(true);
        base.OnMouseUp(); 
    }
}
