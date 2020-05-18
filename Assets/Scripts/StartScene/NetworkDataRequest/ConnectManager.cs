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

    //invoked by button
    public override void SendRequest()
    {
        //already connected
        if (networkManager.socketReady)
        {
            cameraTransition.NextTransition();
            return;
        }
        
        //start connecting
        messageManager.Display("Connecting to server...");
        dataManager.Connect(serverAddress.text, int.Parse(port.text), OnReceiveData);

        //massage display
        if (!networkManager.socketReady)
        {
            messageManager.Display(networkManager.errorMessage);
        }
        else
        {
            messageManager.Hide();
            cameraTransition.NextTransition();
        }
    }

    protected override void OnReceiveData(string data)
    {
        Debug.Log(data);
    }
}
