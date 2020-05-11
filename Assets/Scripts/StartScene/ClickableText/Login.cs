using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : ClickableText
{
    [SerializeField] private GameObject loginWindow; 
    private void OnMouseDown()
    {
        loginWindow.SetActive(true);
    }
}
