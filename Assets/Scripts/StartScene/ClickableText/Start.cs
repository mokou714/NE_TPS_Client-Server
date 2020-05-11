using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : ClickableText
{
    [SerializeField] private GameObject connectWindow;
    private void OnMouseDown()
    {
       connectWindow.SetActive(true);
    }
}
