using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private Text message;

    public void Display(string msg)
    {
        gameObject.SetActive(true);
        message.text = msg;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
