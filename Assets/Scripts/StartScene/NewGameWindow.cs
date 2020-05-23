using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameWindow : MonoBehaviour
{
    public void OnYesClick()
    {
        var newData = new PlayerData();
        newData.userId = DataManager.PlayerData.userId;
        newData.userName = DataManager.PlayerData.userName;
        DataManager.PlayerData = newData;
        SceneManager.LoadScene(1);
    }
    
}
