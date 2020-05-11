using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

public class SignUpManager : MonoBehaviour
{

    [SerializeField] private InputField userId;
    [SerializeField] private InputField playerName;
    [SerializeField] private InputField password;
    [SerializeField] private MessageManager messageManager;
    
    //invoked by button
    public void SignUp()
    {
        if (!NetworkManager.socketReady)
        {
            messageManager.Display("Not connected to the server");
            return;
        }
        
        
        var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password.text));
        var result = DataManager.CheckAccountExist(userId.text);
        if (!result)
        {
            messageManager.Display("Signing up...");
            result = DataManager.SignUp(userId.text, playerName.text, hash);
            messageManager.Hide();
        }
        else
        {
            messageManager.Display("Account already exists");
        }
        
    }
}
