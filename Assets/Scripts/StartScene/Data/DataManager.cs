using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public NetworkManager networkManager;
    public static PlayerData PlayerData;

    private bool _isWaiting;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    public void Connect(string host, int port, Action<string> OnRecieveData)
    {
        networkManager.StartConnection(host,port);
        networkManager.Send("Connect;");
        networkManager.Receive();
        StartCoroutine(WaitingForReceiving(OnRecieveData));
    }

    public void SignUp(string userId, string userName, string password, Action<string> OnRecieveData)
    {

        networkManager.Send("Register;" + userId + ";" + userName +";" + password + ";" );
        networkManager.Receive();
        
        StartCoroutine(WaitingForReceiving(OnRecieveData));
    }
    
    public void VerifyAccount(string userId, string password, Action<string> OnRecieveData)
    {

        networkManager.Send("Verify;" + userId + ";" + password + ";");
        networkManager.Receive();
        StartCoroutine(WaitingForReceiving(OnRecieveData));
    }

    public void CheckAccountExist(string userId, Action<string> OnRecieveData)
    {
        networkManager.Send("Check;" + userId);
        networkManager.Receive();
        StartCoroutine(WaitingForReceiving(OnRecieveData));
    }


    public void LoadPlayerData()
    {
        if (PlayerData.userId == null)
        {
            Debug.Log("Unknown Error: User hasn't logged in");
        }

        networkManager.Send("Load;" + PlayerData.userId);
        networkManager.Receive();

    }

    public void SavePlayerData()
    {
        if(PlayerData != null)
            networkManager.Send("SavePlayerData;" + PlayerData.userId);
        

    }


    private IEnumerator WaitingForReceiving (Action<string> OnRecieveData)
    {
        Debug.Log("Waiting For Receiving...");
        _isWaiting = true;
        yield return new WaitUntil(() => networkManager.finishedReceiving);
        Debug.Log("End Receiving...");
        _isWaiting = false;
        Debug.Log("Fetched Data");
        var data = networkManager.FetchReceivedData();
        OnRecieveData(data);

    }
}
