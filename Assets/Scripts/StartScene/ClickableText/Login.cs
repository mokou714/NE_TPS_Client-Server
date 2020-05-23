using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : ClickableText
{
    [SerializeField] private GameObject loginWindow;
    [SerializeField] private ClickableText signUp;
    [SerializeField] private CanvasBackButton mainBackButton;
    protected override void OnMouseUp()
    {
        if (isClickable && mouseDown)
        {
            loginWindow.SetActive(true);
            isClickable = signUp.isClickable = false;
            mainBackButton.gameObject.SetActive(false);
        }
        base.OnMouseUp();
    }
}
