using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static PlayerData PlayerData = new PlayerData();
    private NetworkManager networkManager;

    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance is null)
        {
            Debug.Log("Created DataManger instance");
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(this);
            Debug.Log("Destroy extra DataManger instance");
        }
    }

    private void Start()
    {
        networkManager = NetworkManager.Instance;
    }

    
    public bool Connect(string host, int port, Action<string> OnRecieveData)
    {
        //send text msg after creating the socket
        if (networkManager.StartConnection(host, port))
        {
            networkManager.Send("Connect;");
            networkManager.Receive();
            StartCoroutine(WaitingForReceiving(OnRecieveData));
            return true;
        }

        return false;
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


    public void LoadPlayerData(Action<string> OnRecieveData)
    {
        networkManager.Send("Load;" + PlayerData.userId);
        networkManager.Receive();
        StartCoroutine(WaitingForReceiving(OnRecieveData));
    }

    public void SavePlayerData(Action<string> OnRecieveData)
    {
        var data = PlayerData.userId + ";" 
                                     + PlayerData.health + ";"
                                     + PlayerData.backupAmmo + ";"
                                     + PlayerData.arrowCount + ";"
                                     + PlayerData.exp + ";"
                                     + PlayerData.totalKills + ";"
                                     + PlayerData.playingTime + ";"
                                     + PlayerData.currentLevel + ";";

        networkManager.Send("Save;" + data);
        networkManager.Receive();
        StartCoroutine(WaitingForReceiving(OnRecieveData));
    }


    private IEnumerator WaitingForReceiving (Action<string> OnRecieveData)
    {
        Debug.Log("Waiting For Receiving...");
        yield return new WaitUntil(() => networkManager.finishedReceiving);
        Debug.Log("End Receiving...");
        Debug.Log("Fetched Data");
        var data = networkManager.FetchReceivedData();
        OnRecieveData(data);
    }
}
