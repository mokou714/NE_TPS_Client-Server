using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : ClickableText
{
    [SerializeField] private GameObject creditWindow;
    protected override void OnMouseUp()
    {
        if (isClickable && mouseDown)
        {
            creditWindow.SetActive(true);
        }
        base.OnMouseUp();
    }
}
