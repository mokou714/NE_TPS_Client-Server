using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private InputField userId;
    [SerializeField] private InputField password;
    [SerializeField] private MessageManager messageManager;

    //invoked by button
    public void Login()
    {
        if (!NetworkManager.socketReady)
        {
           messageManager.Display("Not connected to the server");
            return;
        }

        var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password.text));
        var result = DataManager.VerifyAccount(userId.text, hash);
        
        messageManager.Display("Logging...");
        if(!result) messageManager.Display("Login Failed");
        else messageManager.Hide();
    }
    
}
