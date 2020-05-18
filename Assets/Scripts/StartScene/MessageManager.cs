using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private Text message;
    private List<Action> OnCloseActions = new List<Action>();
    public void Display(string msg)
    {
        gameObject.SetActive(true);
        message.text = msg;
       
    }

    public void SetOnCloseActions(IEnumerable<Action> actions)
    {
        OnCloseActions.AddRange(actions);
    }
    

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnCloseEvent()
    {

        foreach (var action in OnCloseActions)
        {
            action();
        }
        
        OnCloseActions.Clear();
    }


}
