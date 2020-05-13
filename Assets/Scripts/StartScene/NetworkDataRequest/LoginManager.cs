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
    public MessageManager messageManager;
    
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
        
        
        //messageManager.Display("Login Failed");
        //messageManager.Hide();
    }

    protected override void OnReceiveData(string data)
    {
        if (data == "success")
        {
            messageManager.Display("Login succeeded");
        }
        else
        {
            Debug.Log(data);
            messageManager.Display("Login failed");
        }

    }
}
