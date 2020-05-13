using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;

public class SignUpManager : NetworkDataRequest
{
    public InputField userId;
    public InputField playerName;
    public InputField password;
    public MessageManager messageManager;

    private bool accountExists;
    //invoked by button
    public override void SendRequest()
    {
        if (!networkManager.socketReady)
        {
            messageManager.Display("Not connected to the server");
            return;
        }

        messageManager.Display("Signing up...");
        
        // ask if account exists first
        dataManager.CheckAccountExist(userId.text, OnReceiveCheckingResult);
        
        
    }

    private void OnReceiveCheckingResult(string data)
    {
        // user id does not exist, create account
        if (data == "false")
        {
            var sha256 = SHA256.Create();
            var sha256hash = sha256.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password.text));
            var stringBuilder = new StringBuilder();
            foreach(var b in sha256hash)
                stringBuilder.AppendFormat("{0:X2}", b);

            var hashString = stringBuilder.ToString();
            
            dataManager.SignUp(userId.text, playerName.text, hashString, OnReceiveData);
        }
        else
        {
            messageManager.Display("Account already exists");
        }
    }

    protected override void OnReceiveData(string data)
    {
        if (data == "success")
        {
            messageManager.Display("Account created");
        }
        else
        {
            Debug.Log(data);
            messageManager.Display("Failed to sign up");
        }
    }
}
