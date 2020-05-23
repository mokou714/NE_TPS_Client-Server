using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignUp : ClickableText
{
    [SerializeField] private GameObject signUpWindow;
    [SerializeField] private ClickableText login;
    [SerializeField] private CanvasBackButton mainBackButton;
    protected override void OnMouseUp()
    {
        if (isClickable && mouseDown)
        {
            signUpWindow.SetActive(true);
            isClickable = login.isClickable = false;
            mainBackButton.gameObject.SetActive(false);
        }
        base.OnMouseUp();
    }
}
