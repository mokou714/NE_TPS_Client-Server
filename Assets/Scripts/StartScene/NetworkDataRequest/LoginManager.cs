using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;

public class LoginManager : NetworkDataRequest
{
    public InputField userId;
    public InputField password;
    public StatisticsLoader statisticsLoader;
    public ClickableText signUp;
    public ClickableText login;
    public CanvasBackButton mainBackButton;
 
    //invoked by button
    public override void SendRequest()
    {
        if (!networkManager.socketReady)
        {
            messageManager.Display("Not connected to the server");
            return;
        }

        var sha256 = SHA256.Create();
        var sha256hash = sha256.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password.text));
        var stringBuilder = new StringBuilder();
        foreach(var b in sha256hash)
            stringBuilder.AppendFormat("{0:X2}", b);

        var hashString = stringBuilder.ToString();
        
        messageManager.Display("Logging in...");
        dataManager.VerifyAccount(userId.text, hashString, OnReceiveData);
    }

    protected override void OnReceiveData(string data)
    {
        if (data == "success")
        {
            DataManager.PlayerData.userId = userId.text;
            dataManager.LoadPlayerData(OnReceivePlayerData);
        }
        else
        {
            Debug.Log(data);
            signUp.isClickable = login.isClickable = true;
            messageManager.Display("Login failed");
            messageManager.SetOnCloseActions(new List<Action>{()=>{mainBackButton.gameObject.SetActive(true);}});
        }
        
        //Reset inputfields
        userId.text = password.text = "";

    }

    private void OnReceivePlayerData(string playData)
    {    
        //order:
        //userName, health, backupAmmo, arrowCount, exp,
        //totalKills, playingTime, currentLevel
        var tokens = playData.Split(';');
        DataManager.PlayerData.userName = tokens[0];
        DataManager.PlayerData.health = int.Parse(tokens[1]);
        DataManager.PlayerData.backupAmmo = int.Parse(tokens[2]);
        DataManager.PlayerData.arrowCount = int.Parse(tokens[3]);
        DataManager.PlayerData.exp = int.Parse(tokens[4]);
        DataManager.PlayerData.totalKills = int.Parse(tokens[5]);
        DataManager.PlayerData.playingTime = tokens[6];
        DataManager.PlayerData.currentLevel = int.Parse(tokens[7]);

        signUp.isClickable = login.isClickable = true;
        messageManager.Display("Login succeeded");
        messageManager.SetOnCloseActions(new List<Action>{cameraTransition.NextTransition, statisticsLoader.Load, ()=>{mainBackButton.gameObject.SetActive(true);}});
        
    }
    
    
}
