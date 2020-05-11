using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignUp : ClickableText
{
    [SerializeField] private GameObject signUpWindow; 
    private void OnMouseDown()
    {
        signUpWindow.SetActive(true);
    }
}
