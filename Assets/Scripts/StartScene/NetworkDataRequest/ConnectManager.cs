using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : NetworkDataRequest{
    public InputField serverAddress;
    public InputField port;
    public GameObject mainBackButton;
    public GameObject title;
    //invoked by button
    public override void SendRequest()
    {
        //already connected
        if (networkManager.IsReady())
        {
            Debug.Log("Already connected");
            cameraTransition.NextTransition();
            return;
        }
        
        //start connecting
        messageManager.Display("Connecting to server...");
        if (!dataManager.Connect(serverAddress.text, int.Parse(port.text), OnReceiveData))
        {
            messageManager.Display("Failed to build connection with the server");
        }

       
    }

    protected override void OnReceiveData(string data)
    {
        //massage display
        if (data == "success")
        {
            messageManager.Hide();
            cameraTransition.NextTransition();
            mainBackButton.SetActive(true);
            title.SetActive(false);
        }
        else
        {
            messageManager.Display(networkManager.errorMessage);
        }
    }
}
