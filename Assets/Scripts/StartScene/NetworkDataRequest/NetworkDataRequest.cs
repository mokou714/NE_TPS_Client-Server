﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkDataRequest : MonoBehaviour
{
    public NetworkManager networkManager;
    public DataManager dataManager;
    public CameraTransition cameraTransition;
    public MessageManager messageManager;
    
    public abstract void SendRequest();
    
    protected abstract void OnReceiveData(string data);
}
