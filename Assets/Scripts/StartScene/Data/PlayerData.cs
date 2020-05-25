using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string userId;
    public string userName;
    public int health;
    public int backupAmmo;
    public int arrowCount;
    public int exp;
    public int totalKills;
    public string playingTime;
    public int currentLevel;

    public PlayerData()
    {
        userId = "";
        userName = "";
        health = 100;
        backupAmmo = 90;
        arrowCount = 10;
        exp = 0;
        totalKills = 0;
        playingTime = "0h 0m 0s";
        currentLevel = 0;
    }
}
