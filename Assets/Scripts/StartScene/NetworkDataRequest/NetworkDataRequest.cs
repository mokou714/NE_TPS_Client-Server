using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkDataRequest : MonoBehaviour
{
    public CameraTransition cameraTransition;
    public MessageManager messageManager;
    
    protected DataManager dataManager;
    protected NetworkManager networkManager;

    private void Start()
    {
        dataManager = DataManager.Instance;
        networkManager = NetworkManager.Instance;
    }

    public abstract void SendRequest();
    
    protected abstract void OnReceiveData(string data);
}
