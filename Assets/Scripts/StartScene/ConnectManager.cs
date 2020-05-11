using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : MonoBehaviour
{
    [SerializeField] private InputField serverAddress;
    [SerializeField] private InputField port;
    [SerializeField] private MessageManager messageManager;
    [SerializeField] private CameraTransition cameraTransition;
  

    public void Connect()
    {
        if (NetworkManager.socketReady) return;
        
        messageManager.Display("Connecting to server...");
        NetworkManager.StartConnection(serverAddress.text, int.Parse(port.text));

        if (!NetworkManager.socketReady)
        {
            messageManager.Display(NetworkManager.errorMessage);
        }
        else
        {
            messageManager.Hide();
            cameraTransition.NextTransition();
        }
        
    }
    
}
