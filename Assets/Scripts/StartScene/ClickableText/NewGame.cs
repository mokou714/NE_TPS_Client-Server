using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : ClickableText
{
    [SerializeField] private NewGameWindow newGameWindow;
    protected override void OnMouseUp()
    {
        if (isClickable && mouseDown)
        {
            newGameWindow.gameObject.SetActive(true);
        }
        base.OnMouseUp();
    }
}
