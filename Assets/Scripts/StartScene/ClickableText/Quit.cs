using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : ClickableText
{
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (isClickable && mouseDown)
        {
            Application.Quit();
        }
    }
}
