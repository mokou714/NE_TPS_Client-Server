using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static PlayerData PlayerData;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    public static bool SignUp(string userId, string userName, byte[] password)
    {

        NetworkManager.Send("Register;" + userName +";" + password + ";" );
        var result = NetworkManager.Receive();
        if (result == "Succeeded")
            return true;
        if (result == "Failed")
            return false;
        //unknown data from server
        Debug.Log("Unknown data from server");
        return false;
    }

    public static bool VerifyAccount(string userId, byte[] password)
    {

        NetworkManager.Send("Verify;" + userId + ";" + password + ";");
        var result = NetworkManager.Receive();
        if (result == "Succeeded")
            return true;
        if (result == "Failed")
            return false;
        //unknown data from server
        Debug.Log("Unknown data from server");
        return false;
    }

    public static bool CheckAccountExist(string userId)
    {
        NetworkManager.Send("CheckAccount;" + userId);
        var result = NetworkManager.Receive();
        if (result == "Succeeded")
            return true;
        if (result == "Failed")
            return false;
        //unknown data from server
        Debug.Log("Unknown data from server");
        return false;
    }


    public static bool LoadPlayerData()
    {
        if (PlayerData.userId == null)
        {
            Debug.Log("Unknown Error: User hasn't logged in");
            return false;
        }

        NetworkManager.Send("LoadPlayerData;" + PlayerData.userId);

        var data = NetworkManager.Receive();

        Debug.Log(data);
        return true;
    }

    public static bool SavePlayerData()
    {
        if(PlayerData != null)
            NetworkManager.Send("SavePlayerData;" + PlayerData.userId);

        return true;

    }
}
